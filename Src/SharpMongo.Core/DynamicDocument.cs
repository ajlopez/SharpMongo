namespace SharpMongo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DynamicDocument
    {
        private IDictionary<string, object> values = new Dictionary<string, object>();

        public DynamicDocument(params object[] arguments)
        {
            for (int k = 0; k < arguments.Length; k += 2)
                this.values[arguments[k].ToString()] = arguments[k + 1];
        }

        public object GetMember(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            return null;
        }

        public void SetMember(string name, object value)
        {
            this.values[name] = value;
        }
    }
}
