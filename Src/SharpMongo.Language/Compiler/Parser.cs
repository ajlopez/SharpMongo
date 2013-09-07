namespace SharpMongo.Language.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpMongo.Language.Commands;

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
