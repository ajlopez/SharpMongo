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
    }
}
