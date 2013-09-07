namespace SharpMongo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Engine
    {
        IDictionary<string, DocumentBase> dbases = new Dictionary<string, DocumentBase>();

        public DocumentBase GetDocumentBase(string name)
        {
            if (this.dbases.ContainsKey(name))
                return this.dbases[name];

            return null;
        }

        public DocumentBase CreateDocumentBase(string name)
        {
            if (this.dbases.ContainsKey(name))
                throw new InvalidOperationException(string.Format("Document Base '{0}' already exists", name));

            DocumentBase dbase = new DocumentBase(name);
            this.dbases[name] = dbase;

            return dbase;
        }
    }
}
