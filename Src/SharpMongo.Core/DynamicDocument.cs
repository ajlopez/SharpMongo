namespace SharpMongo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DynamicDocument : DynamicObject
    {
        public DynamicDocument(params object[] arguments) 
            : base(arguments)
        {
        }

        internal DynamicDocument(IDictionary<string, object> values)
            : base(values)
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

        public DynamicDocument Clone()
        {
            return new DynamicDocument(this.values);
        }
    }
}
