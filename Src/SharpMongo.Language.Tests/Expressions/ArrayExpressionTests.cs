namespace SharpMongo.Language.Tests.Expressions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Language.Expressions;

    [TestClass]
    public class ArrayExpressionTests
    {
        [TestMethod]
        public void GetArray()
        {
            ArrayExpression expression = new ArrayExpression(new IExpression[] { new ConstantExpression(1), new ConstantExpression(2), new ConstantExpression(3) });

            var result = expression.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<object>));

            var array = (IList<object>)result;

            Assert.AreEqual(3, array.Count);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
        }
    }
}
