namespace SharpMongo.Language.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpMongo.Language.Commands;
    using SharpMongo.Language.Expressions;

    public class Parser
    {
        private Lexer lexer;
        private Stack<Token> tokens = new Stack<Token>();

        public Parser(string text)
        {
            this.lexer = new Lexer(text);
        }

        public ICommand ParseCommand()
        {
            Token token = this.NextToken();

            if (token == null)
                return null;

            if (token.Type == TokenType.Name)
            {
                if (token.Value == "exit")
                    return new ExitCommand();

                if (token.Value == "use")
                    return new UseCommand(this.ParseName());

                if (token.Value == "show")
                {
                    if (this.TryParseName("dbs"))
                        return new ShowDbsCommand();
                    if (this.TryParseName("collections"))
                        return new ShowCollectionsCommand();
                }
            }

            throw new ParserException("Unknown command");
        }

        public IExpression ParseExpression()
        {
            IExpression expr = this.ParseTerm();

            while (true)
            {
                if (this.TryParseToken(".", TokenType.Punctuation))
                {
                    expr = new DotExpression(expr, this.ParseName());
                    continue;
                }

                if (this.TryParseToken("(", TokenType.Punctuation))
                {
                    IList<IExpression> exprs = new List<IExpression>();

                    while (!this.TryParseToken(")", TokenType.Punctuation))
                    {
                        if (exprs.Count > 0)
                            this.ParseToken(",", TokenType.Punctuation);

                        exprs.Add(this.ParseExpression());
                    }

                    expr = new CallExpression(expr, exprs);

                    continue;
                }

                break;
            }

            return expr;
        }

        private IExpression ParseTerm()
        {
            Token token = this.NextToken();

            if (token == null)
                return null;

            if (token.Type == TokenType.Name)
                return new NameExpression(token.Value);

            if (token.Type == TokenType.String)
                return new ConstantExpression(token.Value);

            if (token.Type == TokenType.Integer)
                return new ConstantExpression(int.Parse(token.Value, System.Globalization.CultureInfo.InvariantCulture));

            if (token.Type == TokenType.Real)
                return new ConstantExpression(double.Parse(token.Value, System.Globalization.CultureInfo.InvariantCulture));

            throw new ParserException("Syntax error");
        }

        private bool TryParseName(string name)
        {
            Token token = this.NextToken();

            if (token == null || token.Type != TokenType.Name || token.Value != name)
            {
                this.PushToken(token);
                return false;
            }

            return true;
        }

        private bool TryParseToken(string value, TokenType type)
        {
            Token token = this.NextToken();

            if (token == null || token.Type != type || token.Value != value)
            {
                this.PushToken(token);
                return false;
            }

            return true;
        }

        private void ParseToken(string value, TokenType type)
        {
            Token token = this.NextToken();

            if (token == null || token.Type != type || token.Value != value)
                throw new ParserException(string.Format("Expected '{0}'", value));
        }

        private string ParseName()
        {
            Token token = this.NextToken();

            if (token == null || token.Type != TokenType.Name)
                throw new ParserException("Name expected");

            return token.Value;
        }

        private void PushToken(Token token)
        {
            this.tokens.Push(token);
        }

        private Token NextToken()
        {
            if (this.tokens.Count > 0)
                return this.tokens.Pop();

            return this.lexer.NextToken();
        }
    }
}
