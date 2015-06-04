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

        protected override IList<string> GetInitialNames()
        {
            IList<string> names = new List<string>();

            names.Add("Id");
            return names;
        }

        protected override DynamicObject GetEmptyClone()
        {
            return new DynamicDocument();
        }
    }
}
