namespace SharpMongo.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DocumentBaseTests
    {
        [TestMethod]
        public void CreateWithName()
        {
            DocumentBase dbase = new DocumentBase("Test");

            Assert.AreEqual("Test", dbase.Name);
        }

        [TestMethod]
        public void GetNonExistentCollection()
        {
            DocumentBase dbase = new DocumentBase("Test");

            Assert.IsNull(dbase.GetCollection("Unknown"));
        }

        [TestMethod]
        public void CreateCollection()
        {
            DocumentBase dbase = new DocumentBase("Test");

            var collection = dbase.CreateCollection("People");

            Assert.IsNotNull(collection);
            Assert.AreEqual("People", collection.Name);

            Assert.AreSame(collection, dbase.GetCollection("People"));
        }

        [TestMethod]
        public void CreateCollectionTwice()
        {
            DocumentBase dbase = new DocumentBase("Test");

            dbase.CreateCollection("People");

            try
            {
                dbase.CreateCollection("People");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                Assert.AreEqual("Collection 'People' already exists", ex.Message);
            }
        }

        [TestMethod]
        public void GetOrCreateCollection()
        {
            DocumentBase dbase = new DocumentBase("Test");

            var collection = dbase.GetOrCreateCollection("People");

            Assert.IsNotNull(collection);
            Assert.AreEqual("People", collection.Name);

            Assert.AreSame(collection, dbase.GetCollection("People"));
        }

        [TestMethod]
        public void GetOrCreateCollectionTwice()
        {
            DocumentBase dbase = new DocumentBase("Test");

            var collection = dbase.GetOrCreateCollection("People");

            Assert.IsNotNull(collection);
            Assert.AreEqual("People", collection.Name);

            Assert.AreSame(collection, dbase.GetCollection("People"));

            var collection2 = dbase.GetOrCreateCollection("People");

            Assert.IsNotNull(collection2);
            Assert.AreSame(collection, collection2);
        }

        [TestMethod]
        public void GetCollectionNames()
        {
            DocumentBase dbase = new DocumentBase("Test");

            dbase.CreateCollection("People");
            dbase.CreateCollection("Assets");

            var result = dbase.GetCollectionNames();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Assets", result.First());
            Assert.AreEqual("People", result.Skip(1).First());
        }
    }
}
