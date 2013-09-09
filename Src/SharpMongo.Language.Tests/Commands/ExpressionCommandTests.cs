namespace SharpMongo.Language.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpMongo.Language.Commands;
    using SharpMongo.Language.Expressions;

    [TestClass]
    public class ExpressionCommandTests
    {
        [TestMethod]
        public void GetConstant()
        {
            ExpressionCommand command = new ExpressionCommand(new ConstantExpression(123));

            var result = command.Execute(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void GetName()
        {
            Context context = new Context();
            context.SetMember("one", 1);
            ExpressionCommand command = new ExpressionCommand(new NameExpression("one"));

            var result = command.Execute(context);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
        }
    }
}
