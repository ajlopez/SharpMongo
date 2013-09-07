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

            if (token.Type == TokenType.Name && token.Value == "exit")
                return new ExitCommand();

            throw new ParserException("Unknown command");
        }

        private Token NextToken()
        {
            return this.lexer.NextToken();
        }
    }
}
