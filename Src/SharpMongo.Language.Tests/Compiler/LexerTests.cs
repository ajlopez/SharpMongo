namespace SharpMongo.Language.Tests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Language.Compiler;

    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void GetName()
        {
            Lexer lexer = new Lexer("db");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("db", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetNameWithSpaces()
        {
            Lexer lexer = new Lexer("  db   ");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("db", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetNameWithInitialSign()
        {
            Lexer lexer = new Lexer("$add");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("$add", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetNameWithTwoInitialSigns()
        {
            Lexer lexer = new Lexer("$$add");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("$$add", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetNameWithInitialUnderscore()
        {
            Lexer lexer = new Lexer("_add");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("_add", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetNames()
        {
            Lexer lexer = new Lexer("show dbs");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("show", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("dbs", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetPunctuations()
        {
            string punctuations = "();{}:,.[]";
            Lexer lexer = new Lexer(punctuations);

            for (var k = 0; k < punctuations.Length; k++)
            {
                var result = lexer.NextToken();
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Value.Length);
                Assert.AreEqual(punctuations[k], result.Value[0]);
                Assert.AreEqual(TokenType.Punctuation, result.Type);
            }

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetNameDotName()
        {
            Lexer lexer = new Lexer("db.foo");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("db", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(".", result.Value);
            Assert.AreEqual(TokenType.Punctuation, result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Value);
            Assert.AreEqual(TokenType.Name, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetString()
        {
            Lexer lexer = new Lexer("\"foo\"");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Value);
            Assert.AreEqual(TokenType.String, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetSingleQuotedString()
        {
            Lexer lexer = new Lexer("'foo'");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Value);
            Assert.AreEqual(TokenType.String, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetUnclosedString()
        {
            Lexer lexer = new Lexer("'foo");

            try
            {
                lexer.NextToken();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ParserException));
                Assert.AreEqual("Unclosed string", ex.Message);
            }
        }

        [TestMethod]
        public void GetInteger()
        {
            Lexer lexer = new Lexer("123");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
            Assert.AreEqual(TokenType.Integer, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetIntegerWithSyntaxError()
        {
            Lexer lexer = new Lexer("123m");

            try
            {
                lexer.NextToken();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ParserException));
                Assert.AreEqual("Syntax error", ex.Message);
            }
        }

        [TestMethod]
        public void GetReal()
        {
            Lexer lexer = new Lexer("123.456");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("123.456", result.Value);
            Assert.AreEqual(TokenType.Real, result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetRealWithSyntaxError()
        {
            Lexer lexer = new Lexer("123.456m");

            try
            {
                lexer.NextToken();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ParserException));
                Assert.AreEqual("Syntax error", ex.Message);
            }
        }
    }
}
