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

        [TestMethod]
        public void FindOneDocument()
        {
            Collection collection = new Collection();
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);

            collection.Insert(document);

            var result = collection.Find();

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(document.GetMember("Id"), result.First().GetMember("Id"));
            Assert.AreEqual("Adam", result.First().GetMember("Name"));
            Assert.AreEqual(800, result.First().GetMember("Age"));
        }
    }
}
