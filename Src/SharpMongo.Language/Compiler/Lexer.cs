namespace SharpMongo.Language.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Lexer
    {
        private string text;
        private int length;
        private int position;

        public Lexer(string text)
        {
            this.text = text;
            this.length = text.Length;
            this.position = 0;
        }

        public Token NextToken()
        {
            while (this.position < this.length && char.IsWhiteSpace(this.text[this.position]))
                this.position++;

            if (this.position >= this.length)
                return null;

            string result = this.text[this.position++].ToString();

            while (this.position < this.length && !char.IsWhiteSpace(this.text[this.position]))
                result += this.text[this.position++];

            var token = new Token(result, TokenType.Name);

            return token;
        }
    }
}
