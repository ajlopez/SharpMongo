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
    public class CallExpressionTests
    {
        [TestMethod]
        public void CallFind()
        {
            Collection collection = new Collection("people");
            CollectionObject collobj = new CollectionObject(collection);
            Context context = new Context();
            context.SetMember("people", collobj);

            CallExpression expression = new CallExpression(new DotExpression(new NameExpression("people"), "find"), new IExpression[] { });

            var result = expression.Evaluate(context);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<DynamicDocument>));
        }
    }
}
