namespace SharpMongo.Language.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Language.Expressions;

    [TestClass]
    public class ConstantExpressionTests
    {
        [TestMethod]
        public void EvaluateNull()
        {
            ConstantExpression expression = new ConstantExpression(null);
            Assert.IsNull(expression.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateInteger()
        {
            ConstantExpression expression = new ConstantExpression(123);
            Assert.AreEqual(123, expression.Evaluate(null));
        }
    }
}
