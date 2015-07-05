namespace SharpMongo.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    using Microsoft.VisualBasic.CompilerServices;

    public class DynamicObject : SharpMongo.Core.IObject
    {
        private static Dictionary<string, Func<object, object, object>> operators = new Dictionary<string, Func<object, object, object>>();

        private IList<string> names = new List<string>();
        private IDictionary<string, object> values = new Dictionary<string, object>();
        private bool @sealed;

        static DynamicObject()
        {
            operators["$subtract"] = Operators.SubtractObject;
            operators["$divide"] = Operators.DivideObject;
            operators["$mod"] = Operators.ModObject;
            operators["$concat"] = Operators.ConcatenateObject;
        }

        public DynamicObject(params object[] arguments)
        {
            for (int k = 0; k < arguments.Length; k += 2)
            {
                var name = arguments[k].ToString();
                this.names.Add(name);
                this.values[name] = arguments[k + 1];
            }
        }

        public virtual object GetMember(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            return null;
        }

        public virtual bool Exists(string name)
        {
            return this.values.ContainsKey(name);
        }

        public virtual void SetMember(string name, object value)
        {
            if (this.@sealed)
                throw new InvalidOperationException();

            if (!this.names.Contains(name))
                this.names.Add(name);

            this.values[name] = value;
        }

        public bool Match(DynamicObject document)
        {
            foreach (var key in this.values.Keys)
            {
                var value = this.values[key];

                if (value is DynamicObject)
                    return ((DynamicObject)value).Match(document, key);

                if (!this.values[key].Equals(document.GetMember(key)))
                    return false;
            }

            return true;
        }

        public void Update(DynamicObject document, bool reset = false)
        {
            if (reset)
            {
                object id = this.GetMember("Id");
                this.names = new List<string>();
                this.values = new Dictionary<string, object>();

                if (id != null)
                {
                    this.names.Add("Id");
                    this.values["Id"] = id;
                }
            }

            foreach (var key in document.values.Keys)
            {
                if (!this.names.Contains(key))
                    this.names.Add(key);

                this.values[key] = document.values[key];
            }
        }

        public void Seal()
        {
            this.@sealed = true;
        }

        public IEnumerable<string> GetMemberNames()
        {
            return this.names;
        }

        public string ToJsonString()
        {
            if (this.names.Count == 0)
                return "{ }";

            StringBuilder builder = new StringBuilder();

            builder.Append("{ ");

            int count = 0;

            foreach (string name in this.names)
            {
                if (count > 0)
                    builder.Append(", ");

                count++;

                builder.Append(string.Format("\"{0}\": ", name));
                object value = this.values[name];

                if (value is string)
                    builder.Append(string.Format("\"{0}\"", value));
                else
                    builder.Append(value);
            }

            builder.Append(" }");

            return builder.ToString();
        }

        public DynamicObject Project(DynamicObject projection)
        {
            IList<string> names = this.GetInitialNames();

            return this.Project(projection, names);
        }

        protected virtual IList<string> GetInitialNames()
        {
            IList<string> names = new List<string>();

            return names;
        }

        protected virtual DynamicObject GetEmptyClone()
        {
            return new DynamicObject();
        }

        protected DynamicObject Project(DynamicObject projection, IList<string> names)
        {
            foreach (var name in projection.GetMemberNames().Where(n => IsTrue(projection.GetMember(n))))
                if (!names.Contains(name))
                    names.Add(name);

            if (names.Count == 0 || (names.Count == 1 && names.First() == "Id"))
                names = this.GetMemberNames().ToList();

            foreach (var name in projection.GetMemberNames().Where(n => IsFalse(projection.GetMember(n))))
                if (names.Contains(name))
                    names.Remove(name);

            DynamicObject document = this.GetEmptyClone();

            foreach (var name in names)
                document.SetMember(name, this.GetMember(name));

            foreach (var name in projection.GetMemberNames().Where(n => !IsFalse(projection.GetMember(n)) && !IsTrue(projection.GetMember(n))))
                document.SetMember(name, this.Evaluate(projection.GetMember(name)));

            return document;
        }

        private static bool IsFalse(object value)
        {
            return value != null && (value.Equals(false) || value.Equals(0));
        }

        private static bool IsTrue(object value)
        {
            return value != null && (value.Equals(true) || value.Equals(1));
        }

        private bool Match(DynamicObject dynobj, string name)
        {
            var value = dynobj.GetMember(name);

            foreach (var key in this.values.Keys)
            {
                if (key == "$exists")
                    if (!dynobj.Exists(name).Equals(this.GetMember(key)))
                        return false;
                    else
                        continue;

                if (key == "$lt")
                    if (!(((IComparable)value).CompareTo(this.GetMember(key)) < 0))
                        return false;
                    else
                        continue;

                if (key == "$lte")
                    if (!(((IComparable)value).CompareTo(this.Evaluate(this.GetMember(key))) <= 0))
                        return false;
                    else
                        continue;

                if (key == "$gt")
                    if (!(((IComparable)value).CompareTo(this.GetMember(key)) > 0))
                        return false;
                    else
                        continue;

                if (key == "$gte")
                    if (!(((IComparable)value).CompareTo(this.GetMember(key)) >= 0))
                        return false;
                    else
                        continue;

                if (key == "$ne")
                    if (value.Equals(this.GetMember(key)))
                        return false;
                    else
                        continue;

                if (key == "$eq")
                    if (!value.Equals(this.GetMember(key)))
                        return false;
                    else
                        continue;

                if (key == "$in")
                    if (!((IList)this.GetMember(key)).Contains(value))
                        return false;
                    else
                        continue;

                if (key == "$nin")
                    if (((IList)this.GetMember(key)).Contains(value))
                        return false;
                    else
                        continue;

                if (key == "$not")
                    if (((DynamicObject)this.GetMember(key)).Match(dynobj, name))
                        return false;
                    else
                        continue;

                if (key == "$or")
                {
                    bool result = false;
                    foreach (var dobj in (IEnumerable<DynamicObject>)this.GetMember(key))
                        if (dobj.Match(dynobj, name))
                        {
                            result = true;
                            break;
                        }

                    if (!result)
                        return false;

                    continue;
                }

                if (key == "$nor")
                {
                    foreach (var dobj in (IEnumerable<DynamicObject>)this.GetMember(key))
                        if (dobj.Match(dynobj, name))
                            return false;
                    continue;
                }

                if (key == "$and")
                {
                    foreach (var dobj in (IEnumerable<DynamicObject>)this.GetMember(key))
                        if (!dobj.Match(dynobj, name))
                            return false;
                    continue;
                }

                if (key == "$type")
                {
                    Type type = (Type)this.GetMember(key);

                    if (!type.IsInstanceOfType(value))
                        return false;

                    continue;
                }

                throw new InvalidOperationException(string.Format("Invalid operator '{0}'", key));
            }

            return true;
        }

        private object Evaluate(object value)
        {
            if (!(value is DynamicObject))
                return value;

            var dynobj = (DynamicObject)value;

            foreach (string key in dynobj.GetMemberNames())
                if (operators.ContainsKey(key)) 
                {
                    var values = this.GetValues(dynobj, key);
                    return operators[key](values[0], values[1]);
                }

            if (dynobj.Exists("$eq"))
            {
                var values = this.GetValues(dynobj, "$eq");
                return values[0].Equals(values[1]);
            }

            if (dynobj.Exists("$ne"))
            {
                var values = this.GetValues(dynobj, "$ne");
                return !values[0].Equals(values[1]);
            }

            if (dynobj.Exists("$lt"))
            {
                var values = this.GetValues(dynobj, "$lt");

                return ((IComparable)values[0]).CompareTo(values[1]) < 0;
            }

            if (dynobj.Exists("$lte"))
            {
                var values = this.GetValues(dynobj, "$lte");

                return ((IComparable)values[0]).CompareTo(values[1]) <= 0;
            }

            if (dynobj.Exists("$gt"))
            {
                var values = this.GetValues(dynobj, "$gt");

                return ((IComparable)values[0]).CompareTo(values[1]) > 0;
            }

            if (dynobj.Exists("$gte"))
            {
                var values = this.GetValues(dynobj, "$gte");

                return ((IComparable)values[0]).CompareTo(values[1]) >= 0;
            }

            if (dynobj.Exists("$cmp"))
            {
                var values = this.GetValues(dynobj, "$cmp");

                return ((IComparable)values[0]).CompareTo(values[1]);
            }

            if (dynobj.Exists("$strcasecmp"))
            {
                var values = this.GetValues(dynobj, "$strcasecmp");

                return values[0].ToString().ToLowerInvariant().CompareTo(values[1].ToString().ToLowerInvariant());
            }

            if (dynobj.Exists("$add"))
            {
                var values = this.GetValues(dynobj, "$add");
                var value1 = values.First();
                var value2 = values.Skip(1).First();

                var result = Operators.AddObject(value1, value2);

                foreach (var val in values.Skip(2))
                    result = Operators.AddObject(result, val);

                return result;
            }

            if (dynobj.Exists("$multiply"))
            {
                var values = this.GetValues(dynobj, "$multiply");
                var value1 = values.First();
                var value2 = values.Skip(1).First();

                var result = Operators.MultiplyObject(value1, value2);

                foreach (var val in values.Skip(2))
                    result = Operators.MultiplyObject(result, val);

                return result;
            }

            if (dynobj.Exists("$tolower"))
            {
                var val = this.GetValue(dynobj.GetMember("$tolower"));

                return val.ToString().ToLowerInvariant();
            }

            if (dynobj.Exists("$toupper"))
            {
                var val = this.GetValue(dynobj.GetMember("$toupper"));

                return val.ToString().ToUpperInvariant();
            }

            if (dynobj.Exists("$literal"))
                return dynobj.GetMember("$literal");

            return null;
        }

        private object GetValue(object value)
        {
            if (value is string)
            {
                string name = (string)value;

                if (name.Length > 0 && name[0] == '$')
                    return this.GetMember(name.Substring(1));
            }

            if (value is DynamicObject)
                return this.Evaluate(value);

            return value;
        }

        private IList<object> GetValues(DynamicObject dynobj, string name)
        {
            var vals = (IEnumerable<object>)dynobj.GetMember(name);
            IList<object> values = new List<object>();

            foreach (var val in vals)
                values.Add(this.GetValue(val));

            return values;
        }
    }
}
