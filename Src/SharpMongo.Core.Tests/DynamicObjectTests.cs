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
        public void Seal()
        {
            DynamicObject document = new DynamicObject("Name", "Adam", "Age", 800);
            document.Seal();

            try
            {
                document.SetMember("Age", 700);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
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
            DynamicObject obj = new DynamicObject("Name", "Adam", "Age", 800);
            DynamicObject update = new DynamicObject("Height", 180, "Age", 700);

            obj.Update(update);

            Assert.AreEqual(180, obj.GetMember("Height"));
            Assert.AreEqual(700, obj.GetMember("Age"));
        }

        [TestMethod]
        public void GetMemberNames()
        {
            DynamicObject obj = new DynamicObject("Name", "Adam", "Age", 800, "Wife", "Eve" );

            var result = obj.GetMemberNames().ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Name", result[0]);
            Assert.AreEqual("Age", result[1]);
            Assert.AreEqual("Wife", result[2]);
        }

        [TestMethod]
        public void ToJsonString()
        {
            DynamicObject obj = new DynamicObject("Name", "Adam", "Age", 800, "Wife", "Eve");

            var result = obj.ToJsonString();

            Assert.IsNotNull(result);
            Assert.AreEqual("{ \"Name\": \"Adam\", \"Age\": 800, \"Wife\": \"Eve\" }", result);
        }

        [TestMethod]
        public void EmptyToJsonString()
        {
            DynamicObject obj = new DynamicObject();

            var result = obj.ToJsonString();

            Assert.IsNotNull(result);
            Assert.AreEqual("{ }", result);
        }
    }
}
