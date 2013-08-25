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

        public DynamicDocument Project(IEnumerable<string> names)
        {
            DynamicDocument document = new DynamicDocument();

            foreach (var name in names)
                document.SetMember(name, this.GetMember(name));

            return document;
        }
    }
}
