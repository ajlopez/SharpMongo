namespace SharpMongo.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DynamicObjectTests
    {
        [TestMethod]
        public void CreateAndGetUndefinedMember()
        {
            DynamicObject document = new DynamicObject();

            Assert.IsNull(document.GetMember("Name"));
        }

        [TestMethod]
        public void GetDefinedMember()
        {
            DynamicObject document = new DynamicObject("Name", "Adam");

            Assert.AreEqual("Adam", document.GetMember("Name"));
        }

        [TestMethod]
        public void SetAndGetMember()
        {
            DynamicObject document = new DynamicObject();

            document.SetMember("Name", "Eve");

            Assert.AreEqual("Eve", document.GetMember("Name"));
        }

        [TestMethod]
        public void MatchOnePropertyQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", 800);

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void DontMatchOnePropertyQuery()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject query = new DynamicObject("Age", 700);

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void UpdateExistingProperty()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject update = new DynamicObject("Age", 700);

            document.Update(update);

            Assert.AreEqual(700, document.GetMember("Age"));
        }

        [TestMethod]
        public void UpdateNotExistingProperty()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject update = new DynamicObject("Height", 180);

            document.Update(update);

            Assert.AreEqual(180, document.GetMember("Height"));
        }

        [TestMethod]
        public void UpdateProperties()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject update = new DynamicObject("Height", 180, "Age", 700);

            document.Update(update);

            Assert.AreEqual(180, document.GetMember("Height"));
            Assert.AreEqual(700, document.GetMember("Age"));
        }
    }
}
