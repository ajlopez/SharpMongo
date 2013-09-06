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
            document.Id = id;
            document.Seal();
            this.documents.Add(document);
            this.documentsbyid[id] = document;
        }

        public DynamicDocument GetDocument(Guid id)
        {
            if (!this.documentsbyid.ContainsKey(id))
                return null;

            return this.documentsbyid[id];
        }

        public IEnumerable<DynamicDocument> Find(DynamicObject query = null, DynamicObject projection = null)
        {
            if (projection != null)
            {
                IEnumerable<string> names = projection.GetMemberNames();

                if (query == null)
                    foreach (var document in this.documents)
                        yield return document.Project(projection);
                else
                    foreach (var document in this.documents)
                        if (query.Match(document))
                            yield return document.Project(projection);
            }
            else
            {
                if (query == null)
                    foreach (var document in this.documents)
                        yield return document;
                else
                    foreach (var document in this.documents)
                        if (query.Match(document))
                            yield return document;
            }
        }

        public void Update(DynamicObject query, DynamicObject update, bool multi = false)
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
