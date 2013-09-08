namespace SharpMongo.Language.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Lexer
    {
        private static string punctuations = "();{}:";

        private string text;
        private int length;
        private int position;

        public Lexer(string text)
        {
            this.text = text;
            this.length = text == null ? 0 : text.Length;
            this.position = 0;
        }

        public Token NextToken()
        {
            while (this.position < this.length && char.IsWhiteSpace(this.text[this.position]))
                this.position++;

            if (this.position >= this.length)
                return null;

            char ch = this.text[this.position++];

            if (ch == '"' || ch == '\'')
                return NextString(ch);

            string result = ch.ToString();

            if (punctuations.Contains(ch))
                return new Token(result, TokenType.Punctuation);

            while (this.position < this.length && !char.IsWhiteSpace(this.text[this.position]))
                result += this.text[this.position++];

            var token = new Token(result, TokenType.Name);

            return token;
        }

        private Token NextString(char delimiter)
        {
            string result = string.Empty;

            while (this.position < this.length && this.text[this.position] != delimiter)
                result += this.text[this.position++];

            if (this.position >= this.length)
                throw new ParserException("Unclosed string");

            this.position++;

            return new Token(result, TokenType.String);
        }
    }
}
