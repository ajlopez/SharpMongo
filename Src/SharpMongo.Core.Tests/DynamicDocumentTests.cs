namespace SharpMongo.Core.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DynamicDocumentTests
    {
        [TestMethod]
        public void ProjectName()
        {
            DynamicDocument document = new DynamicDocument("Name", "Adam", "Age", 800);

            var result = document.Project(new string[] { "Name" });

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.GetMember("Name"));
            Assert.AreEqual("Adam", result.GetMember("Name"));
            Assert.IsNull(result.GetMember("Age"));
            Assert.AreEqual(1, result.GetMemberNames().Count());
            Assert.AreEqual("Name", result.GetMemberNames().First());
        }
    }
}
