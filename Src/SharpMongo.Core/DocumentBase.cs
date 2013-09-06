namespace SharpMongo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DocumentBase
    {
        private string name;
        private IDictionary<string, Collection> collections = new Dictionary<string, Collection>();

        public DocumentBase(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public Collection GetCollection(string name)
        {
            if (this.collections.ContainsKey(name))
                return this.collections[name];

            return null;
        }

        public Collection CreateCollection(string name)
        {
            if (this.collections.ContainsKey(name))
                throw new InvalidOperationException(string.Format("Collection '{0}' already exists", name));

            var collection = new Collection(name);
            this.collections[name] = collection;

            return collection;
        }
    }
}
