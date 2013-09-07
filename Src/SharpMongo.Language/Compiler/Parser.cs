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
                }
            }

            throw new ParserException("Unknown command");
        }

        private bool TryParseName(string name)
        {
            Token token = this.NextToken();

            if (token == null || token.Type != TokenType.Name || token.Value != name)
                return false;

            return true;
        }

        private string ParseName()
        {
            Token token = this.NextToken();

            if (token == null || token.Type != TokenType.Name)
                throw new ParserException("Name expected");

            return token.Value;
        }

        private Token NextToken()
        {
            return this.lexer.NextToken();
        }
    }
}
