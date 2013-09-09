namespace SharpMongo.Language.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpMongo.Core;

    public class DotExpression : IExpression
    {
        private IExpression expression;
        private string name;

        public DotExpression(IExpression expression, string name)
        {
            this.expression = expression;
            this.name = name;
        }

        public IExpression Expression { get { return this.expression; } }

        public string Name { get { return this.name; } }

        public object Evaluate(Context context)
        {
            IObject obj = (IObject)this.expression.Evaluate(context);
            return obj.GetMember(this.name);
        }
    }
}
