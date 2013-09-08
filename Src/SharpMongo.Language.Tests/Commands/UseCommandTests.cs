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
    public class UseCommandTests
    {
        [TestMethod]
        public void UseNewDocumentBase()
        {
            Context context = new Context();
            context.Engine = new Engine();

            UseCommand command = new UseCommand("Genesis");

            command.Execute(context);

            Assert.IsNotNull(context.DocumentBase);
            Assert.AreEqual("Genesis", context.DocumentBase.Name);
            Assert.IsNotNull(context.Engine.GetDocumentBase("Genesis"));
            Assert.AreSame(context.DocumentBase, context.Engine.GetDocumentBase("Genesis"));
        }
    }
}
