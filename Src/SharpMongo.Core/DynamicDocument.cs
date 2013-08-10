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
                values[arguments[k].ToString()] = arguments[k + 1];
        }

        public object GetMember(string name)
        {
            if (values.ContainsKey(name))
                return values[name];

            return null;
        }
    }
}
