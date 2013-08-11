namespace SharpMongo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Collection
    {
        private IList<DynamicDocument> documents = new List<DynamicDocument>();
        private IDictionary<Guid, DynamicDocument> documentsbyid = new Dictionary<Guid, DynamicDocument>();

        public void Insert(DynamicDocument document)
        {
            Guid id = Guid.NewGuid();
            document.SetMember("Id", id);
            this.documents.Add(document);
            this.documentsbyid[id] = document;
        }

        public DynamicDocument GetDocument(Guid id)
        {
            if (!this.documentsbyid.ContainsKey(id))
                return null;

            return this.documentsbyid[id];
        }

        public IEnumerable<DynamicDocument> Find()
        {
            foreach (var document in this.documents)
                yield return document;
        }

        public IEnumerable<DynamicDocument> Find(DynamicDocument query)
        {
            if (query.GetMember("Id") != null)
            {
                var document = this.GetDocument((Guid)query.GetMember("Id"));

                if (document != null && query.Match(document))
                    yield return document;
            }
            else
                foreach (var document in this.documents)
                    if (query.Match(document))
                        yield return document;
        }

        public void Update(DynamicDocument query, DynamicDocument update, bool multi = false)
        {
            foreach (var document in this.Find(query))
            {
                document.Update(update);

                if (!multi)
                    return;
            }
        }
    }
}

