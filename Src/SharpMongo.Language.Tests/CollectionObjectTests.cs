namespace SharpMongo.Language.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Core;

    [TestClass]
    public class CollectionObjectTests
    {
        private DynamicDocument adam;

        [TestMethod]
        public void CallInsertObject()
        {
            Collection collection = new Collection("people");
            CollectionObject collobj = new CollectionObject(collection);
            IFunction insertmth = (IFunction)collobj.GetMember("insert");
            insertmth.Apply(new object[] { new DynamicObject("Name", "Adam", "Age", 800) });

            var result = collection.Find();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            var document = result.First();

            Assert.AreEqual("Adam", document.GetMember("Name"));
            Assert.AreEqual(800, document.GetMember("Age"));
            Assert.IsNotNull(document.Id);
            Assert.AreEqual(3, document.GetMemberNames().Count());
        }

        [TestMethod]
        public void CallFind()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction findmth = (IFunction)collobj.GetMember("find");

            var result = findmth.Apply(new object[] { });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<DynamicDocument>));
            Assert.AreEqual(4, ((IEnumerable<DynamicDocument>)result).Count());
        }

        [TestMethod]
        public void CallFindWithQuery()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction findmth = (IFunction)collobj.GetMember("find");

            var result = findmth.Apply(new object[] { new DynamicObject("Age", 800) });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<DynamicDocument>));
            Assert.AreEqual(1, ((IEnumerable<DynamicDocument>)result).Count());

            var document = ((IEnumerable<DynamicDocument>)result).First();

            Assert.AreEqual(800, document.GetMember("Age"));
        }

        [TestMethod]
        public void CallFindWithProjection()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction findmth = (IFunction)collobj.GetMember("find");

            var result = findmth.Apply(new object[] { null, new DynamicObject("Age", 1) });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<DynamicDocument>));

            var documents = (IEnumerable<DynamicDocument>)result;

            Assert.AreEqual(4, documents.Count());

            Assert.IsTrue(documents.All(d => d.GetMember("Age") != null));
            Assert.IsTrue(documents.All(d => d.GetMember("Id") != null));
            Assert.IsTrue(documents.All(d => d.GetMember("Name") == null));
            Assert.IsTrue(documents.All(d => d.GetMemberNames().Count() == 2));
        }

        [TestMethod]
        public void CallFindOneWithQuery()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction findonemth = (IFunction)collobj.GetMember("findOne");

            var result = findonemth.Apply(new object[] { new DynamicObject("Age", 800) });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicDocument));

            var document = (DynamicDocument)result;

            Assert.AreEqual(800, document.GetMember("Age"));
        }

        [TestMethod]
        public void CallFindOneWithProjection()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction findonemth = (IFunction)collobj.GetMember("findOne");

            var result = findonemth.Apply(new object[] { null, new DynamicObject("Age", 1) });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicDocument));

            var document = (DynamicDocument)result;

            Assert.IsNotNull(document.GetMember("Age"));
            Assert.IsNotNull(document.GetMember("Id"));
            Assert.IsNull(document.GetMember("Name"));
            Assert.AreEqual(2, document.GetMemberNames().Count());
        }

        [TestMethod]
        public void CallUpdate()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction updatemth = (IFunction)collobj.GetMember("update");

            updatemth.Apply(new object[] { new DynamicObject("Age", 700), new DynamicObject("Name", "New Eve", "Age", 700) });

            var result = collection.Find(new DynamicObject("Age", 700)).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(700, result.GetMember("Age"));
            Assert.AreEqual("New Eve", result.GetMember("Name"));
        }

        [TestMethod]
        public void CallUpdateWithMulti()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction updatemth = (IFunction)collobj.GetMember("update");

            updatemth.Apply(new object[] { new DynamicObject("Age", 700), new DynamicObject("Name", "New Eve", "Age", 700), true });

            var result = collection.Find(new DynamicObject("Age", 700)).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(700, result.GetMember("Age"));
            Assert.AreEqual("New Eve", result.GetMember("Name"));
        }

        [TestMethod]
        public void CallUpdateAllWithMulti()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction updatemth = (IFunction)collobj.GetMember("update");

            updatemth.Apply(new object[] { null, new DynamicObject("Kind", "Human"), true });

            var result = collection.Find();

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());

            Assert.IsTrue(result.All(d => d.GetMemberNames().Contains("Kind")));
            Assert.IsTrue(result.All(d => d.GetMember("Kind").Equals("Human")));
        }

        [TestMethod]
        public void CallCount()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction countmth = (IFunction)collobj.GetMember("count");

            var result = countmth.Apply(new object[] { });

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void CallCountWithQueryCriteria()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction countmth = (IFunction)collobj.GetMember("count");

            var result = countmth.Apply(new object[] { new DynamicObject("Age", 700) });

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CallRemoveOne()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction removemth = (IFunction)collobj.GetMember("remove");

            removemth.Apply(new object[] { new DynamicObject("Age", 700) });

            var result = collection.Find();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());

            Assert.IsTrue(result.All(d => !d.GetMember("Age").Equals(700)));
        }

        [TestMethod]
        public void CallRemoveAll()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction removemth = (IFunction)collobj.GetMember("remove");

            removemth.Apply(new object[] { });

            var result = collection.Find();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void CallRemoveFirst()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction removemth = (IFunction)collobj.GetMember("remove");

            removemth.Apply(new object[] { null, true });

            var result = collection.Find();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void CallSaveNewDocument()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction savemth = (IFunction)collobj.GetMember("save");

            savemth.Apply(new object[] { new DynamicObject("Name", "Set", "Age", 300) });

            var result = collection.Find(new DynamicObject("Name", "Set")).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual("Set", result.GetMember("Name"));
            Assert.AreEqual(300, result.GetMember("Age"));
            Assert.IsNotNull(result.Id);
        }

        [TestMethod]
        public void CallSaveExistingDocument()
        {
            Collection collection = this.GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction savemth = (IFunction)collobj.GetMember("save");

            savemth.Apply(new object[] { new DynamicObject("Id", this.adam.Id, "Name", "Adam", "Age", 300) });

            var result = collection.Find(new DynamicObject("Name", "Adam")).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.AreEqual(300, result.GetMember("Age"));
            Assert.AreEqual(this.adam.Id, result.Id);
        }

        private Collection GetCollection()
        {
            var collection = new Collection("People");

            this.adam = new DynamicDocument("Name", "Adam", "Age", 800);
            var eve = new DynamicDocument("Name", "Eve", "Age", 700);
            var cain = new DynamicDocument("Name", "Cain", "Age", 600);
            var abel = new DynamicDocument("Name", "Abel", "Age", 500);

            collection.Insert(this.adam);
            collection.Insert(eve);
            collection.Insert(cain);
            collection.Insert(abel);

            return collection;
        }
    }
}
