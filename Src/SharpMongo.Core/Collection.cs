namespace SharpMongo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Collection
    {
        private IList<DynamicDocument> documents = new List<DynamicDocument>();

        public void Insert(DynamicDocument document)
        {
            document.SetMember("Id", Guid.NewGuid());
            this.documents.Add(document);
        }

        public IEnumerable<DynamicDocument> Find()
        {
            foreach (var document in this.documents)
                yield return document;
        }

        public IEnumerable<DynamicDocument> Find(DynamicDocument query)
        {
            foreach (var document in this.documents)
                if (query.Match(document))
                    yield return document;
        }
    }
}

