namespace SharpMongo.Language.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CallExpression : IExpression
    {
        private IExpression expression;
        private IList<IExpression> arguments;

        public CallExpression(IExpression expression, IList<IExpression> arguments)
        {
            this.expression = expression;
            this.arguments = arguments;
        }

        public IExpression Expression { get { return this.expression; } }

        public IList<IExpression> Arguments { get { return this.arguments; } }
    }
}
