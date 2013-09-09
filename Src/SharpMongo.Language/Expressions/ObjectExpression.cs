namespace SharpMongo.Language.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpMongo.Core;

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

        public object Evaluate(Context context)
        {
            DynamicObject dobj = new DynamicObject();
            int k = 0;

            foreach (var name in this.names)
                dobj.SetMember(name, this.expressions[k++].Evaluate(context));

            return dobj;
        }
    }
}
