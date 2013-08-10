namespace SharpMongo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Collection
    {
        public void Insert(DynamicDocument document)
        {
            document.SetMember("Id", Guid.NewGuid());
        }
    }
}
