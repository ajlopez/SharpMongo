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

            string result = ch.ToString();

            if (punctuations.Contains(ch))
                return new Token(result, TokenType.Punctuation);

            while (this.position < this.length && !char.IsWhiteSpace(this.text[this.position]))
                result += this.text[this.position++];

            var token = new Token(result, TokenType.Name);

            return token;
        }
    }
}
