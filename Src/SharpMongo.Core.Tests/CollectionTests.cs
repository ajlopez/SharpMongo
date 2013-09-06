namespace SharpMongo.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CollectionTests
    {
        [TestMethod]
        public void InsertDocument()
        {
            Collection collection = new Collection("Test");
            DynamicDocument document = new DynamicDocument();

            collection.Insert(document);

            Assert.IsNotNull(document.Id);
            Assert.AreEqual(document.Id, document.GetMember("Id"));
            Assert.IsInstanceOfType(document.Id, typeof(Guid));
        }

        [TestMethod]
        public void SaveNewDocument()
        {
            Collection collection = new Collection("Test");
            DynamicDocument document = new DynamicDocument();

            collection.Save(document);

            Assert.IsNotNull(document.Id);
            Assert.AreEqual(document.Id, document.GetMember("Id"));
            Assert.IsInstanceOfType(document.Id, typeof(Guid));
        }

        [TestMethod]
        public void SaveNewDocumentWithId()
        {
            Collection collection = new Collection("Test");
            var id = Guid.NewGuid();
            DynamicDocument document = new DynamicDocument() { Id = id };

            collection.Save(document);

            var result = collection.Find(new DynamicDocument() { Id = id });

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            var newdocument = result.First();
            Assert.AreEqual(id, newdocument.Id);
        }

        [TestMethod]
        public void SaveExistingDocument()
        {
            Collection collection = new Collection("Test");
            DynamicDocument original = new DynamicDocument("Name", "Adam");

            collection.Insert(original);

            DynamicDocument document = new DynamicDocument("Name", "New Adam", "Age", 800) { Id = original.Id };
            collection.Save(document);

            var result = collection.Find();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            var newdocument = result.First();

            Assert.IsNotNull(newdocument);
            Assert.AreEqual(original.Id, newdocument.Id);
            Assert.AreEqual("New Adam", newdocument.GetMember("Name"));
            Assert.AreEqual(800, newdocument.GetMember("Age"));
        }

        [TestMethod]
        public void InsertAndModifyDocument()
        {
            Collection collection = new Collection("Test");
            DynamicDocument document = new DynamicDocument("Name", "Adam");

            collection.Insert(document);

            try
            {
                document.SetMember("Name", "Eve");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void FindAndModifyDocument()
        {
            Collection collection = new Collection("Test");
            DynamicDocument document = new DynamicDocument("Name", "Adam");

            collection.Insert(document);
            var newdocument = collection.Find(new DynamicDocument() { Id = document.Id }).First();

            try
            {
                newdocument.SetMember("Name", "Eve");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void FindOneDocument()
        {
            Collection collection = new Collection("Test");
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);

            collection.Insert(document);

            var result = collection.Find();

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(document.GetMember("Id"), result.First().GetMember("Id"));
            Assert.AreEqual("Adam", result.First().GetMember("Name"));
            Assert.AreEqual(800, result.First().GetMember("Age"));
        }

        [TestMethod]
        public void GetDocument()
        {
            Collection collection = new Collection("Test");
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);

            collection.Insert(document);

            var result = collection.GetDocument((Guid)document.GetMember("Id"));

            Assert.IsNotNull(result);

            Assert.AreEqual(document.GetMember("Id"), result.GetMember("Id"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.AreEqual(800, result.GetMember("Age"));
        }

        [TestMethod]
        public void GetUnknownDocument()
        {
            Collection collection = new Collection("Test");

            Assert.IsNull(collection.GetDocument(Guid.NewGuid()));
        }

        [TestMethod]
        public void FindOneDocumentUsingQuery()
        {
            Collection collection = new Collection("Test");
            DynamicDocument document1 = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument document2 = new DynamicDocument("Name", "Eve", "Age", 700);

            collection.Insert(document1);
            collection.Insert(document2);

            var result = collection.Find(new DynamicDocument("Age", 700));

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(document2.GetMember("Id"), result.First().GetMember("Id"));
            Assert.AreEqual("Eve", result.First().GetMember("Name"));
            Assert.AreEqual(700, result.First().GetMember("Age"));
        }

        [TestMethod]
        public void FindOneDocumentUsingQueryWithId()
        {
            Collection collection = new Collection("Test");
            DynamicDocument document1 = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument document2 = new DynamicDocument("Name", "Eve", "Age", 700);

            collection.Insert(document1);
            collection.Insert(document2);

            var result = collection.Find(new DynamicDocument("Id", document2.GetMember("Id"), "Age", 700));

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(document2.GetMember("Id"), result.First().GetMember("Id"));
            Assert.AreEqual("Eve", result.First().GetMember("Name"));
            Assert.AreEqual(700, result.First().GetMember("Age"));
        }

        [TestMethod]
        public void FindOneDocumentUsingQueryWithIdAndDifferentValue()
        {
            Collection collection = new Collection("Test");
            DynamicDocument document1 = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument document2 = new DynamicDocument("Name", "Eve", "Age", 700);

            collection.Insert(document1);
            collection.Insert(document2);

            var result = collection.Find(new DynamicDocument("Id", document2.GetMember("Id"), "Age", 800));

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void FindUnknownDocumentUsingQueryWithId()
        {
            Collection collection = new Collection("Test");

            var result = collection.Find(new DynamicDocument("Id", Guid.NewGuid()));

            Assert.AreEqual(0, result.Count());
        }
    }
}
