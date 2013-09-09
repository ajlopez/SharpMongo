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
    public class DotExpressionTests
    {
        [TestMethod]
        public void GetName()
        {
            IObject dobj = new DynamicObject("Name", "Adam", "Age", 800);
            Context context = new Context();
            context.SetMember("adam", dobj);

            DotExpression expression = new DotExpression(new NameExpression("adam"), "Name");

            var result = expression.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.AreEqual("Adam", result);
        }
    }
}
