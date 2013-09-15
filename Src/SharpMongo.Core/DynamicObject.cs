namespace SharpMongo.Core
{
    using System;
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
                if (!this.values[key].Equals(document.GetMember(key)))
                    return false;

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
    }
}
