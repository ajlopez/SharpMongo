namespace SharpMongo.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DynamicDocument : DynamicObject
    {
        public DynamicDocument(params object[] arguments) 
            : base(arguments)
        {
        }

        public object Id
        {
            get
            {
                return this.GetMember("Id");
            }

            set
            {
                this.SetMember("Id", value);
            }
        }

        public DynamicDocument Project(DynamicObject projection)
        {
            IList<string> names = new List<string>();

            names.Add("Id");

            foreach (var name in projection.GetMemberNames().Where(n => IsTrue(projection.GetMember(n))))
                if (!names.Contains(name))
                    names.Add(name);

            if (names.Count == 1)
                names = this.GetMemberNames().ToList();

            foreach (var name in projection.GetMemberNames().Where(n => IsFalse(projection.GetMember(n))))
                if (names.Contains(name))
                    names.Remove(name);

            DynamicDocument document = new DynamicDocument();

            foreach (var name in names)
                document.SetMember(name, this.GetMember(name));

            foreach (var name in  projection.GetMemberNames().Where(n => !IsFalse(projection.GetMember(n)) && !IsTrue(projection.GetMember(n))))
                document.SetMember(name, this.Evaluate(projection.GetMember(name)));

            return document;
        }

        private object Evaluate(object value)
        {
            if (!(value is DynamicObject))
                return value;

            var dynobj = (DynamicObject)value;

            if (dynobj.Exists("$eq"))
            {
                var values = (IEnumerable<object>)dynobj.GetMember("$eq");
                var value1 = this.GetValue(values.First());
                var value2 = this.GetValue(values.Skip(1).First());

                return value1.Equals(value2);
            }

            if (dynobj.Exists("$ne"))
            {
                var values = (IEnumerable<object>)dynobj.GetMember("$ne");
                var value1 = this.GetValue(values.First());
                var value2 = this.GetValue(values.Skip(1).First());

                return !value1.Equals(value2);
            }

            if (dynobj.Exists("$lt"))
            {
                var values = (IEnumerable<object>)dynobj.GetMember("$lt");
                var value1 = this.GetValue(values.First());
                var value2 = this.GetValue(values.Skip(1).First());

                return ((IComparable)value1).CompareTo(value2) < 0;
            }

            if (dynobj.Exists("$lte"))
            {
                var values = (IEnumerable<object>)dynobj.GetMember("$lte");
                var value1 = this.GetValue(values.First());
                var value2 = this.GetValue(values.Skip(1).First());

                return ((IComparable)value1).CompareTo(value2) <= 0;
            }

            if (dynobj.Exists("$gt"))
            {
                var values = (IEnumerable<object>)dynobj.GetMember("$gt");
                var value1 = this.GetValue(values.First());
                var value2 = this.GetValue(values.Skip(1).First());

                return ((IComparable)value1).CompareTo(value2) > 0;
            }

            if (dynobj.Exists("$gte"))
            {
                var values = (IEnumerable<object>)dynobj.GetMember("$gte");
                var value1 = this.GetValue(values.First());
                var value2 = this.GetValue(values.Skip(1).First());

                return ((IComparable)value1).CompareTo(value2) >= 0;
            }

            if (dynobj.Exists("$cmp"))
            {
                var values = (IEnumerable<object>)dynobj.GetMember("$cmp");
                var value1 = this.GetValue(values.First());
                var value2 = this.GetValue(values.Skip(1).First());

                return ((IComparable)value1).CompareTo(value2);
            }

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

            return value;
        }

        private static bool IsFalse(object value)
        {
            return value != null && (value.Equals(false) || value.Equals(0));
        }

        private static bool IsTrue(object value)
        {
            return value != null && (value.Equals(true) || value.Equals(1));
        }
    }
}
