namespace SharpMongo.Language.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ArrayExpression : IExpression
    {
        private IList<IExpression> expressions;

        public ArrayExpression(IList<IExpression> expressions)
        {
            this.expressions = expressions;
        }

        public IEnumerable<IExpression> Expressions { get { return this.expressions; } }
    }
}
