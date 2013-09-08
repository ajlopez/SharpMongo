namespace SharpMongo.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpMongo.Core;

    public class Context
    {
        private IDictionary<string, object> values = new Dictionary<string, object>();

        public Engine Engine { get; set; }

        public DocumentBase DocumentBase { get; set; }

        public void SetMember(string name, object value)
        {
            this.values[name] = value;
        }

        public object GetMember(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            return null;
        }
    }
}
