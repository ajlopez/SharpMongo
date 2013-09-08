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

        public object Evaluate(Context context)
        {
            IList<object> result = new List<object>();

            foreach (var expression in this.expressions)
                result.Add(expression.Evaluate(context));

            return result;
        }
    }
}
