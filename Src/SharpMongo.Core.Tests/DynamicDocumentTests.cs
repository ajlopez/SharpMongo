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
    }
}
