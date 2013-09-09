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
    public class ShowDbsCommandTests
    {
        [TestMethod]
        public void NoDocumentBase()
        {
            Context context = new Context();
            context.Engine = new Engine();
            ShowDbsCommand command = new ShowDbsCommand();

            var result = command.Execute(context);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<string>));

            var names = (IList<string>)result;

            Assert.AreEqual(0, names.Count);
        }

        [TestMethod]
        public void TwoDocumentBases()
        {
            Context context = new Context();
            context.Engine = new Engine();
            context.Engine.CreateDocumentBase("Genesis");
            context.Engine.CreateDocumentBase("Deuteronomius");
            ShowDbsCommand command = new ShowDbsCommand();

            var result = command.Execute(context);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<string>));

            var names = (IList<string>)result;

            Assert.AreEqual(2, names.Count);
            Assert.AreEqual("Deuteronomius", names[0]);
            Assert.AreEqual("Genesis", names[1]);
        }
    }
}
