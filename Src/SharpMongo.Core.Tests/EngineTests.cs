namespace SharpMongo.Core.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EngineTests
    {
        [TestMethod]
        public void GetUnknownDocumentBase()
        {
            Engine engine = new Engine();

            Assert.IsNull(engine.GetDocumentBase("Unknown"));
        }

        [TestMethod]
        public void CreateDocumentBase()
        {
            Engine engine = new Engine();

            var result = engine.CreateDocumentBase("Genesis");

            Assert.IsNotNull(result);
            Assert.AreEqual("Genesis", result.Name);
        }

        [TestMethod]
        public void CreateDocumentBaseTwice()
        {
            Engine engine = new Engine();

            engine.CreateDocumentBase("Genesis");

            try
            {
                engine.CreateDocumentBase("Genesis");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                Assert.AreEqual("Document Base 'Genesis' already exists", ex.Message);
            }
        }

        [TestMethod]
        public void GetDocumentBase()
        {
            Engine engine = new Engine();

            var dbase = engine.CreateDocumentBase("Genesis");

            var result = engine.GetDocumentBase("Genesis");

            Assert.IsNotNull(result);
            Assert.AreEqual("Genesis", result.Name);
            Assert.AreSame(dbase, result);
        }
    }
}
