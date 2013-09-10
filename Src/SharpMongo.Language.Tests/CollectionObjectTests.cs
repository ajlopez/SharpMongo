namespace SharpMongo.Language.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Core;
    using System.Collections;

    [TestClass]
    public class CollectionObjectTests
    {
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
            Collection collection = GetCollection();

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
            Collection collection = GetCollection();

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
            Collection collection = GetCollection();

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
        public void CallUpdate()
        {
            Collection collection = GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction updatemth = (IFunction)collobj.GetMember("update");

            updatemth.Apply(new object[] { new DynamicObject("Age", 700), new DynamicObject("Name", "New Eve") });

            var result = collection.Find(new DynamicObject("Age", 700)).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(700, result.GetMember("Age"));
            Assert.AreEqual("New Eve", result.GetMember("Name"));
        }

        [TestMethod]
        public void CallUpdateWithMulti()
        {
            Collection collection = GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction updatemth = (IFunction)collobj.GetMember("update");

            updatemth.Apply(new object[] { new DynamicObject("Age", 700), new DynamicObject("Name", "New Eve"), true });

            var result = collection.Find(new DynamicObject("Age", 700)).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(700, result.GetMember("Age"));
            Assert.AreEqual("New Eve", result.GetMember("Name"));
        }

        [TestMethod]
        public void CallUpdateAllWithMulti()
        {
            Collection collection = GetCollection();

            CollectionObject collobj = new CollectionObject(collection);
            IFunction updatemth = (IFunction)collobj.GetMember("update");

            updatemth.Apply(new object[] { null, new DynamicObject("Kind", "Human"), true });

            var result = collection.Find();

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());

            Assert.IsTrue(result.All(d => d.GetMemberNames().Contains("Kind")));
            Assert.IsTrue(result.All(d => d.GetMember("Kind").Equals("Human")));
        }

        private Collection GetCollection()
        {
            var  collection = new Collection("People");

            var adam = new DynamicDocument("Name", "Adam", "Age", 800);
            var eve = new DynamicDocument("Name", "Eve", "Age", 700);
            var cain = new DynamicDocument("Name", "Cain", "Age", 600);
            var abel = new DynamicDocument("Name", "Abel", "Age", 500);

            collection.Insert(adam);
            collection.Insert(eve);
            collection.Insert(cain);
            collection.Insert(abel);

            return collection;
        }
    }
}
