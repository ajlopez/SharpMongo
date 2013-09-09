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
