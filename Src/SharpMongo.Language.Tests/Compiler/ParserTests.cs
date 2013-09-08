namespace SharpMongo.Language.Tests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Language.Commands;
    using SharpMongo.Language.Compiler;
    using SharpMongo.Language.Expressions;

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
        public void ParseUseCommand()
        {
            Parser parser = new Parser("use genesis");

            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UseCommand));

            var command = (UseCommand)result;

            Assert.AreEqual("genesis", command.Name);

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseUseCommandWithoutName()
        {
            Parser parser = new Parser("use");

            try
            {
                parser.ParseCommand();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ParserException));
                Assert.AreEqual("Name expected", ex.Message);
            }
        }

        [TestMethod]
        public void ParseShowDbsCommand()
        {
            Parser parser = new Parser("show dbs");

            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ShowDbsCommand));

            Assert.IsNull(parser.ParseCommand());
        }

        [TestMethod]
        public void ParseShowCollectionsCommand()
        {
            Parser parser = new Parser("show collections");

            var result = parser.ParseCommand();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ShowCollectionsCommand));

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

        [TestMethod]
        public void ParseShowAsUnknownCommand()
        {
            Parser parser = new Parser("show");

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

        [TestMethod]
        public void ParseNameExpression()
        {
            Parser parser = new Parser("db");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NameExpression));

            var expr = (NameExpression)result;

            Assert.AreEqual("db", expr.Name);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseNullExpression()
        {
            Parser parser = new Parser(string.Empty);

            var result = parser.ParseExpression();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ParseSyntaxErrorInExpression()
        {
            Parser parser = new Parser("}");

            try
            {
                parser.ParseExpression();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ParserException));
                Assert.AreEqual("Syntax error", ex.Message);
            }
        }

        [TestMethod]
        public void ParseStringExpression()
        {
            Parser parser = new Parser("'foo'");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConstantExpression));

            var expr = (ConstantExpression)result;

            Assert.AreEqual("foo", expr.Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseIntegerExpression()
        {
            Parser parser = new Parser("123");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConstantExpression));

            var expr = (ConstantExpression)result;

            Assert.AreEqual(123, expr.Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseRealExpression()
        {
            Parser parser = new Parser("123.456");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConstantExpression));

            var expr = (ConstantExpression)result;

            Assert.AreEqual(123.456, expr.Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseDotExpression()
        {
            Parser parser = new Parser("db.foo");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DotExpression));

            var expr = (DotExpression)result;

            Assert.AreEqual("foo", expr.Name);
            Assert.IsNotNull(expr.Expression);
            Assert.IsInstanceOfType(expr.Expression, typeof(NameExpression));
            Assert.AreEqual("db", ((NameExpression)expr.Expression).Name);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseCallExpression()
        {
            Parser parser = new Parser("help()");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CallExpression));

            var expr = (CallExpression)result;

            Assert.IsNotNull(expr.Expression);
            Assert.IsInstanceOfType(expr.Expression, typeof(NameExpression));
            Assert.IsNotNull(expr.Arguments);
            Assert.AreEqual(0, expr.Arguments.Count);
            Assert.AreEqual("help", ((NameExpression)expr.Expression).Name);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseDotCallExpression()
        {
            Parser parser = new Parser("db.foo.find()");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CallExpression));

            var expr = (CallExpression)result;

            Assert.IsNotNull(expr.Expression);
            Assert.IsInstanceOfType(expr.Expression, typeof(DotExpression));
            Assert.IsNotNull(expr.Arguments);
            Assert.AreEqual(0, expr.Arguments.Count);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseDotCallDotCallExpression()
        {
            Parser parser = new Parser("db.foo().find()");

            var result = parser.ParseExpression();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CallExpression));

            var expr = (CallExpression)result;

            Assert.IsNotNull(expr.Expression);
            Assert.IsInstanceOfType(expr.Expression, typeof(DotExpression));
            Assert.IsNotNull(expr.Arguments);
            Assert.AreEqual(0, expr.Arguments.Count);

            Assert.IsNull(parser.ParseExpression());
        }
    }
}
