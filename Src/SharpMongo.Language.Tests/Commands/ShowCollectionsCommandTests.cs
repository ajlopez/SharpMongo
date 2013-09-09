namespace SharpMongo.Language.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Core;
    using SharpMongo.Language.Commands;

    [TestClass]
    public class ShowCollectionsCommandTests
    {
        [TestMethod]
        public void NoDocumentBase()
        {
            Context context = new Context();
            context.Engine = new Engine();

            ShowCollectionsCommand command = new ShowCollectionsCommand();

            var result = command.Execute(context);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<string>));

            var names = (IList<string>)result;

            Assert.AreEqual(0, names.Count);
        }

        [TestMethod]
        public void NoCollection()
        {
            Context context = new Context();
            context.Engine = new Engine();
            context.DocumentBase = context.Engine.CreateDocumentBase("Genesis");

            ShowCollectionsCommand command = new ShowCollectionsCommand();

            var result = command.Execute(context);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<string>));

            var names = (IList<string>)result;

            Assert.AreEqual(0, names.Count);
        }

        [TestMethod]
        public void TwoCollections()
        {
            Context context = new Context();
            context.Engine = new Engine();
            context.DocumentBase = context.Engine.CreateDocumentBase("Genesis");
            context.DocumentBase.CreateCollection("people");
            context.DocumentBase.CreateCollection("assets");

            ShowCollectionsCommand command = new ShowCollectionsCommand();

            var result = command.Execute(context);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<string>));

            var names = (IList<string>)result;

            Assert.AreEqual(2, names.Count);
            Assert.AreEqual("assets", names[0]);
            Assert.AreEqual("people", names[1]);
        }
    }
}
