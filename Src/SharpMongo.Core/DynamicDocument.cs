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

        public DynamicDocument Project(DynamicObject projection)
        {
            IList<string> names = new List<string>();

            names.Add("Id");

            foreach (var name in projection.GetMemberNames().Where(n => !IsFalse(projection.GetMember(n))))
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

            return document;
        }

        private static bool IsFalse(object value)
        {
            return value == null || value.Equals(false) || value.Equals(0);
        }
    }
}
