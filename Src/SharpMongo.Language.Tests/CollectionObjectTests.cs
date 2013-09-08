namespace SharpMongo.Language.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Core;

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
    }
}
