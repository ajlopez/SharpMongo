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
        public void ProjectNameWithImplicitId()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument projection = new DynamicDocument("Name", true);

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNull(result.GetMember("Age"));
            Assert.AreEqual(2, result.GetMemberNames().Count());
            Assert.IsTrue(result.GetMemberNames().Contains("Name"));
            Assert.IsTrue(result.GetMemberNames().Contains("Id"));
        }

        [TestMethod]
        public void ProjectNameExcludingId()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument projection = new DynamicDocument("Name", true, "Id", false);

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNull(result.GetMember("Age"));
            Assert.AreEqual(1, result.GetMemberNames().Count());
            Assert.IsTrue(result.GetMemberNames().Contains("Name"));
        }

        [TestMethod]
        public void ProjectNewField()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument projection = new DynamicDocument("NewField", 1000);

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("NewField"));
            Assert.AreEqual(1000, result.GetMember("NewField"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpression()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument projection = new DynamicDocument("Has800", new DynamicDocument("$eq", new object[] { "$Age", 800 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("Has800"));
            Assert.AreEqual(true, result.GetMember("Has800"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionNotEqual()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument projection = new DynamicDocument("HasNot800", new DynamicDocument("$ne", new object[] { "$Age", 800 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("HasNot800"));
            Assert.AreEqual(false, result.GetMember("HasNot800"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionLessThan()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument projection = new DynamicDocument("LessThan800", new DynamicDocument("$lt", new object[] { "$Age", 800 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("LessThan800"));
            Assert.AreEqual(false, result.GetMember("LessThan800"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionLessThanOrEqual()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument projection = new DynamicDocument("LessThanOrEqual800", new DynamicDocument("$lte", new object[] { "$Age", 800 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("LessThanOrEqual800"));
            Assert.AreEqual(true, result.GetMember("LessThanOrEqual800"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionGreaterThan()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument projection = new DynamicDocument("GreaterThan600", new DynamicDocument("$gt", new object[] { "$Age", 600 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("GreaterThan600"));
            Assert.AreEqual(true, result.GetMember("GreaterThan600"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectNewFieldWithExpressionGreaterThanOrEqual()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument projection = new DynamicDocument("GreaterThanOrEqual800", new DynamicDocument("$gte", new object[] { "$Age", 800 }));

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNotNull(result.GetMember("Age"));
            Assert.AreEqual(800, result.GetMember("Age"));
            Assert.IsNotNull(result.GetMember("GreaterThanOrEqual800"));
            Assert.AreEqual(true, result.GetMember("GreaterThanOrEqual800"));
            Assert.AreEqual(3, result.GetMemberNames().Count());
        }

        [TestMethod]
        public void ProjectExcludingAge()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);
            DynamicDocument projection = new DynamicDocument("Age", false);

            var result = document.Project(projection);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNull(result.GetMember("Age"));
            Assert.AreEqual(1, result.GetMemberNames().Count());
            Assert.AreEqual("Name", result.GetMemberNames().First());
        }
    }
}
