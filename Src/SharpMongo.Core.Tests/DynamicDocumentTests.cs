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
    }
}
