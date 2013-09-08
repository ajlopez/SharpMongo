﻿namespace SharpMongo.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpMongo.Core;

    public class CollectionObject : DynamicObject
    {
        private Collection collection;

        public CollectionObject(Collection collection)
        {
            this.collection = collection;
            this.SetMember("insert", new InsertMethod(this));
        }

        public Collection Collection { get { return this.collection; } }

        private class InsertMethod : IFunction
        {
            private CollectionObject self;

            public InsertMethod(CollectionObject self)
            {
                this.self = self;
            }

            public object Apply(IList<object> arguments)
            {
                IObject dobj = (IObject)arguments[0];

                DynamicDocument doc = new DynamicDocument();

                foreach (var name in dobj.GetMemberNames())
                    doc.SetMember(name, dobj.GetMember(name));

                if (doc.Id == null)
                    doc.Id = Guid.NewGuid();

                this.self.Collection.Insert(doc);

                return null;
            }
        }
    }
}