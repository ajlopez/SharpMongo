namespace SharpMongo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Collection
    {
        private string name;
        private IList<DynamicDocument> documents = new List<DynamicDocument>();
        private IDictionary<object, DynamicDocument> documentsbyid = new Dictionary<object, DynamicDocument>();

        public Collection(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public void Insert(DynamicDocument document)
        {
            Guid id = Guid.NewGuid();
            document.Id = id;
            document.Seal();
            this.documents.Add(document);
            this.documentsbyid[id] = document;
        }

        public void Save(DynamicDocument document)
        {
            DynamicDocument original = null;

            if (document.Id != null && this.documentsbyid.ContainsKey(document.Id))
                original = this.documentsbyid[document.Id];

            if (original == null)
            {
                if (document.Id == null) 
                {
                    Guid id = Guid.NewGuid();
                    document.Id = id;
                }

                document.Seal();
                this.documents.Add(document);
                this.documentsbyid[document.Id] = document;
            }
            else
            {
                this.documents.Remove(original);
                this.documents.Add(document);
                this.documentsbyid[document.Id] = document;
            }
        }

        public DynamicDocument GetDocument(Guid id)
        {
            if (!this.documentsbyid.ContainsKey(id))
                return null;

            return this.documentsbyid[id];
        }

        public DynamicDocument FindOne(DynamicObject query = null, DynamicObject projection = null)
        {
            return this.Find(query, projection).FirstOrDefault();
        }

        public IEnumerable<DynamicDocument> Find(DynamicObject query = null, DynamicObject projection = null)
        {
            if (projection != null)
            {
                if (query == null)
                    foreach (var document in this.documents)
                        yield return (DynamicDocument)document.Project(projection);
                else
                    foreach (var document in this.documents)
                        if (query.Match(document))
                            yield return (DynamicDocument)document.Project(projection);
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

        public IEnumerable<DynamicObject> Aggregate(DynamicObject spec = null)
        {
            DynamicObject query = null;
            DynamicObject projection = null;

            if (spec != null && spec.Exists("$match"))
                query = (DynamicObject)spec.GetMember("$match");

            if (spec != null && spec.Exists("$project"))
                projection = (DynamicObject)spec.GetMember("$project");

            IEnumerable<DynamicObject> result = this.Find(query, projection);

            if (spec != null && spec.Exists("$skip"))
            {
                int n = (int)spec.GetMember("$skip");

                result = result.Skip(n);
            }

            if (spec != null && spec.Exists("$limit"))
            {
                int n = (int)spec.GetMember("$limit");

                result = result.Take(n);
            }

            if (spec != null && spec.Exists("$sort"))
            {
                DynamicObject sspec = (DynamicObject)spec.GetMember("$sort");
                string[] fldnames = sspec.GetMemberNames().ToArray();
                List<int> orders = new List<int>();

                foreach (var fldname in fldnames)
                    orders.Add((int)sspec.GetMember(fldname) < 0 ? -1 : 1);

                int[] fldorders = orders.ToArray();

                return result.OrderBy(dobj => dobj, new DynamicObjectComparer(fldnames, fldorders));
            }

            return result;
        }

        public void Update(DynamicObject query, DynamicObject update, bool multi = false)
        {
            foreach (var document in this.Find(query))
            {
                document.Update(update, true);

                if (!multi)
                    return;
            }
        }

        public void Remove(DynamicObject query = null, bool justone = false)
        {
            IList<object> toremove = new List<object>();

            foreach (var document in this.Find(query))
            {
                toremove.Add(document.Id);

                if (justone)
                    break;
            }

            foreach (var id in toremove)
            {
                var document = this.documentsbyid[id];
                this.documents.Remove(document);
                this.documentsbyid.Remove(id);
            }
        }

        public int Count(DynamicObject query = null)
        {
            if (query != null)
                return this.Find(query).Count();

            return this.documents.Count;
        }

        public IEnumerable<object> Distinct(string field, DynamicObject query = null)
        {
            return this.Find(query).Select(d => d.GetMember(field)).Distinct();
        }

        private class DynamicObjectComparer : IComparer<DynamicObject>
        {
            private string[] names;
            private int[] orders;
            private int size;

            public DynamicObjectComparer(string[] names, int[] orders)
            {
                this.names = names;
                this.orders = orders;
                this.size = names.Length;
            }

            public int Compare(DynamicObject x, DynamicObject y)
            {
                for (int k = 0; k < this.size; k++)
                {
                    string name = this.names[k];
                    int order =this.orders[k];

                    object value1 = x.GetMember(name);
                    object value2 = y.GetMember(name);

                    int result = ((IComparable)value1).CompareTo(value2);

                    if (result != 0)
                        return result * order;
                }

                return 0;
            }
        }
    }
}
