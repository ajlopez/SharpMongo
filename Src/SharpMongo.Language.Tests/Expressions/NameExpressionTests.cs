namespace SharpMongo.Language.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Language.Expressions;

    [TestClass]
    public class NameExpressionTests
    {
        [TestMethod]
        public void GetNullOnUndefined()
        {
            Context context = new Context();
            NameExpression expression = new NameExpression("foo");

            Assert.IsNull(expression.Evaluate(context));
        }

        [TestMethod]
        public void GetValue()
        {
            Context context = new Context();
            context.SetMember("one", 1);
            NameExpression expression = new NameExpression("one");

            Assert.AreEqual(1, expression.Evaluate(context));
        }
    }
}
