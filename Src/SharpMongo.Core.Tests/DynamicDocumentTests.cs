namespace SharpMongo.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DynamicDocumentTests
    {
        [TestMethod]
        public void CreateAndGetUndefinedMember()
        {
            DynamicDocument document = new DynamicDocument();

            Assert.IsNull(document.GetMember("Name"));
        }

        [TestMethod]
        public void GetDefinedMember()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam");

            Assert.AreEqual("Adam", document.GetMember("Name"));
        }

        [TestMethod]
        public void SetAndGetMember()
        {
            DynamicDocument document = new DynamicDocument();

            document.SetMember("Name", "Eve");

            Assert.AreEqual("Eve", document.GetMember("Name"));
        }

        [TestMethod]
        public void MatchOnePropertyQuery()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument query = new DynamicDocument("Age", 800);

            Assert.IsTrue(query.Match(document));
        }

        [TestMethod]
        public void DontMatchOnePropertyQuery()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument query = new DynamicDocument("Age", 700);

            Assert.IsFalse(query.Match(document));
        }

        [TestMethod]
        public void UpdateExistingProperty()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument update = new DynamicDocument("Age", 700);

            document.Update(update);

            Assert.AreEqual(700, document.GetMember("Age"));
        }

        [TestMethod]
        public void UpdateNotExistingProperty()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument update = new DynamicDocument("Height", 180);

            document.Update(update);

            Assert.AreEqual(180, document.GetMember("Height"));
        }

        [TestMethod]
        public void UpdateProperties()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument update = new DynamicDocument("Height", 180, "Age", 700);

            document.Update(update);

            Assert.AreEqual(180, document.GetMember("Height"));
            Assert.AreEqual(700, document.GetMember("Age"));
        }
    }
}
