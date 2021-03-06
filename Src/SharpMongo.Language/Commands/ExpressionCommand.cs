﻿namespace SharpMongo.Language.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpMongo.Language.Expressions;

    public class ExpressionCommand : ICommand
    {
        private IExpression expression;

        public ExpressionCommand(IExpression expression)
        {
            this.expression = expression;
        }

        public IExpression Expression { get { return this.expression; } }

        public object Execute(Context context)
        {
            return this.expression.Evaluate(context);
        }
    }
}
