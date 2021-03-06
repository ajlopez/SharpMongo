﻿namespace SharpMongo.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PeopleCollectionTests
    {
        private Collection collection;
        private DynamicDocument adam;
        private DynamicDocument eve;
        private DynamicDocument abel;
        private DynamicDocument cain;

        [TestInitialize]
        public void Setup()
        {
            this.collection = new Collection("People");

            this.adam = new DynamicDocument("Name", "Adam", "Age", 800);
            this.eve = new DynamicDocument("Name", "Eve", "Age", 700);
            this.cain = new DynamicDocument("Name", "Cain", "Age", 600);
            this.abel = new DynamicDocument("Name", "Abel", "Age", 500);

            this.collection.Insert(this.adam);
            this.collection.Insert(this.eve);
            this.collection.Insert(this.cain);
            this.collection.Insert(this.abel);
        }

        [TestMethod]
        public void IdsInDocuments()
        {
            Assert.IsNotNull(this.adam.Id);
            Assert.IsNotNull(this.eve.Id);
            Assert.IsNotNull(this.cain.Id);
            Assert.IsNotNull(this.abel.Id);
        }

        [TestMethod]
        public void Count()
        {
            Assert.AreEqual(4, this.collection.Count());
        }

        [TestMethod]
        public void CountWithQueryCriteria()
        {
            Assert.AreEqual(1, this.collection.Count(new DynamicObject("Id", this.eve.Id)));
        }

        [TestMethod]
        public void UpdateAgeInOneDocument()
        {
            this.collection.Update(new DynamicObject("Id", this.eve.Id), new DynamicObject("Age", 600));

            var result = this.collection.Find(new DynamicObject("Id", this.eve.Id));

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(this.eve.Id, result.First().GetMember("Id"));
            Assert.IsNull(result.First().GetMember("Name"));
            Assert.AreEqual(600, result.First().GetMember("Age"));
            Assert.AreEqual(2, result.First().GetMemberNames().Count());

            result = this.collection.Find(new DynamicDocument("Id", this.adam.GetMember("Id")));

            Assert.AreEqual(1, result.Count());

            Assert.AreEqual(this.adam.Id, result.First().GetMember("Id"));
            Assert.AreEqual("Adam", result.First().GetMember("Name"));
            Assert.AreEqual(800, result.First().GetMember("Age"));
        }

        [TestMethod]
        public void UpdateAgeInAllDocuments()
        {
            this.collection.Update(new DynamicDocument(), new DynamicDocument("Age", 600), true);

            var result = this.collection.Find();

            Assert.AreEqual(4, result.Count());

            foreach (var document in result)
                Assert.AreEqual(600, document.GetMember("Age"));
        }

        [TestMethod]
        public void UpdateKindInAllDocuments()
        {
            this.collection.Update(null, new DynamicDocument("Kind", "human"), true);

            var result = this.collection.Find();

            Assert.AreEqual(4, result.Count());

            Assert.IsTrue(result.All(d => d.GetMemberNames().Contains("Kind")));
            Assert.IsTrue(result.All(d => d.GetMember("Kind").Equals("human")));
        }

        [TestMethod]
        public void UpdateAgeInAllDocumentsUsingNullQuery()
        {
            this.collection.Update(null, new DynamicDocument("Age", 600), true);

            var result = this.collection.Find();

            Assert.AreEqual(4, result.Count());

            foreach (var document in result)
                Assert.AreEqual(600, document.GetMember("Age"));
        }

        [TestMethod]
        public void FindAllWithProjection()
        {
            var result = this.collection.Find(null, new DynamicObject("Id", 1, "Name", 1));

            Assert.AreEqual(4, result.Count());

            foreach (var document in result)
            {
                Assert.IsNotNull(document.Id);
                Assert.IsNotNull(document.GetMember("Name"));
                Assert.IsNull(document.GetMember("Age"));
            }
        }

        [TestMethod]
        public void FindAllWithProjectionWithImplicitId()
        {
            var result = this.collection.Find(null, new DynamicObject("Name", 1));

            Assert.AreEqual(4, result.Count());

            foreach (var document in result)
            {
                Assert.IsNotNull(document.Id);
                Assert.IsNotNull(document.GetMember("Name"));
                Assert.IsNull(document.GetMember("Age"));
            }
        }

        [TestMethod]
        public void FindAllWithProjectionExcludingAge()
        {
            var result = this.collection.Find(null, new DynamicObject("Age", 0));

            Assert.AreEqual(4, result.Count());

            foreach (var document in result)
            {
                Assert.IsNotNull(document.Id);
                Assert.IsNotNull(document.GetMember("Name"));
                Assert.IsNull(document.GetMember("Age"));
            }
        }

        [TestMethod]
        public void FindOneWithProjection()
        {
            var result = this.collection.Find(new DynamicObject("Age", 600), new DynamicObject("Id", 1, "Name", 1));

            Assert.AreEqual(1, result.Count());

            var document = result.First();

            Assert.IsNotNull(document.Id);
            Assert.AreEqual("Cain", document.GetMember("Name"));
            Assert.IsNull(document.GetMember("Age"));
            Assert.IsFalse(document.GetMemberNames().Contains("Age"));
        }

        [TestMethod]
        public void FindOneWithProjectionWithLiteral()
        {
            var result = this.collection.Find(new DynamicObject("Age", 600), new DynamicObject("NewField", new DynamicObject("$literal", "NewValue")));

            Assert.AreEqual(1, result.Count());

            var document = result.First();

            Assert.IsNotNull(document.Id);
            Assert.AreEqual("Cain", document.GetMember("Name"));
            Assert.AreEqual(600, document.GetMember("Age"));
            Assert.AreEqual("NewValue", document.GetMember("NewField"));
        }

        [TestMethod]
        public void FindAllWithProjectionWithNewAge()
        {
            var result = this.collection.Find(null, new DynamicObject("NewAge", new DynamicObject("$add", new object[] { "$Age", 1 })));

            Assert.AreEqual(4, result.Count());

            var document = result.First();

            Assert.AreEqual(800, document.GetMember("Age"));
            Assert.AreEqual(801, document.GetMember("NewAge"));

            document = result.Skip(1).First();

            Assert.AreEqual(700, document.GetMember("Age"));
            Assert.AreEqual(701, document.GetMember("NewAge"));

            document = result.Skip(2).First();

            Assert.AreEqual(600, document.GetMember("Age"));
            Assert.AreEqual(601, document.GetMember("NewAge"));

            document = result.Skip(3).First();

            Assert.AreEqual(500, document.GetMember("Age"));
            Assert.AreEqual(501, document.GetMember("NewAge"));
        }

        [TestMethod]
        public void RemoveCain()
        {
            this.collection.Remove(new DynamicObject("Name", "Cain"));

            var result = this.collection.Find();

            Assert.AreEqual(3, result.Count());

            foreach (var document in result)
                Assert.AreNotEqual("Cain", document.GetMember("Name"));
        }

        [TestMethod]
        public void RemoveAll()
        {
            this.collection.Remove();

            var result = this.collection.Find();

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void RemoveFirstOne()
        {
            this.collection.Remove(null, true);

            var result = this.collection.Find();

            Assert.AreEqual(3, result.Count());
        }
    }
}
