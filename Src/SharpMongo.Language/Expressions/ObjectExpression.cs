namespace SharpMongo.Language.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ObjectExpression : IExpression
    {
        private IList<string> names;
        private IList<IExpression> expressions;

        public ObjectExpression(IList<string> names, IList<IExpression> expressions)
        {
            this.names = names;
            this.expressions = expressions;
        }

        public IEnumerable<string> Names { get { return this.names; } }

        public IEnumerable<IExpression> Expressions { get { return this.expressions; } }
    }
}
