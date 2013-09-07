namespace SharpMongo.Language.Tests.Compiler
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Language.Compiler;
    using SharpMongo.Language.Commands;

    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParseNullCommandWhenEmptyString()
        {
            Parser parser = new Parser(string.Empty);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseNullCommandWhenNullString()
        {
            Parser parser = new Parser(null);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseExitCommand()
        {
            Parser parser = new Parser("exit");

            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ExitCommand));

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseUnknownCommand()
        {
            Parser parser = new Parser("foo");

            try
            {
                parser.ParseCommand();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ParserException));
                Assert.AreEqual("Unknown command", ex.Message);
            }
        }
    }
}
