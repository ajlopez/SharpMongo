namespace SharpMongo.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpMongo.Core;

    public class DbObject : DynamicObject
    {
        private DocumentBase dbase;

        public DbObject(DocumentBase dbase)
        {
            this.dbase = dbase;
        }

        public DocumentBase DocumentBase { get { return this.dbase; } }

        public override object GetMember(string name)
        {
            return new CollectionObject(this.dbase.GetOrCreateCollection(name));
        }
    }
}
