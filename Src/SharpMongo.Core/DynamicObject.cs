namespace SharpMongo.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DynamicObject : SharpMongo.Core.IObject
    {
        private IList<string> names = new List<string>();
        private IDictionary<string, object> values = new Dictionary<string, object>();
        private bool @sealed;

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
                    return ((DynamicObject)value).Match(document.GetMember(key));

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

        private bool Match(object value)
        {
            foreach (var key in this.values.Keys)
            {
                if (key == "$lt")
                    if (!(((IComparable)value).CompareTo(this.GetMember(key)) < 0))
                        return false;
                    else
                        continue;

                if (key == "$lte")
                    if (!(((IComparable)value).CompareTo(this.GetMember(key)) <= 0))
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
                    if (((DynamicObject)this.GetMember(key)).Match(value))
                        return false;
                    else
                        continue;

                if (key == "$or")
                {
                    foreach (var dynobj in (IEnumerable<DynamicObject>)this.GetMember(key))
                        if (dynobj.Match(value))
                            return true;
                    return false;
                }

                if (key == "$nor")
                {
                    foreach (var dynobj in (IEnumerable<DynamicObject>)this.GetMember(key))
                        if (dynobj.Match(value))
                            return false;
                    return true;
                }

                if (key == "$and")
                {
                    foreach (var dynobj in (IEnumerable<DynamicObject>)this.GetMember(key))
                        if (!dynobj.Match(value))
                            return false;
                    return true;
                }

                throw new InvalidOperationException(string.Format("Invalid operator '{0}'", key));
            }

            return true;
        }
    }
}
