namespace SharpMongo.Core.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CollectionTests
    {
        [TestMethod]
        public void InsertDocument()
        {
            Collection collection = new Collection();
            DynamicDocument document = new DynamicDocument();

            collection.Insert(document);

            Assert.IsNotNull(document.GetMember("Id"));
            Assert.IsInstanceOfType(document.GetMember("Id"), typeof(Guid));
        }
    }
}
