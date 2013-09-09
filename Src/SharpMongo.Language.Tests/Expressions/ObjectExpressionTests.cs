namespace SharpMongo.Language.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Core;
    using SharpMongo.Language.Expressions;

    [TestClass]
    public class ObjectExpressionTests
    {
        [TestMethod]
        public void GetObject()
        {
            ObjectExpression expression = new ObjectExpression(new string[] { "Name", "Age" }, new IExpression[] { new ConstantExpression("Adam"), new ConstantExpression(800) });

            var result = expression.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IObject));

            var dobj = (IObject)result;

            Assert.AreEqual("Adam", dobj.GetMember("Name"));
            Assert.AreEqual(800, dobj.GetMember("Age"));
            Assert.AreEqual(2, dobj.GetMemberNames().Count());
        }
    }
}
