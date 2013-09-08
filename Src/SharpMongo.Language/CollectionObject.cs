namespace SharpMongo.Language
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
        }

        public Collection Collection { get { return this.collection; } }
    }
}
