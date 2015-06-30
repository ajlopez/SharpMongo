namespace SharpMongo.Language.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Lexer
    {
        private static string punctuations = "();{}[]:,.";

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
                return this.NextString(ch);

            if (char.IsDigit(ch))
                return this.NextInteger(ch);

            string result = ch.ToString();

            if (punctuations.Contains(ch))
                return new Token(result, TokenType.Punctuation);

            if (char.IsLetter(ch) || ch == '$' || ch == '_')
                return this.NextName(ch);

            throw new ParserException(string.Format("Unexpected '{0}'", ch));
        }

        private Token NextName(char letter)
        {
            string result = letter.ToString();

            while (this.position < this.length && (char.IsLetter(this.text[this.position]) || this.text[this.position] == '$'))
                result += this.text[this.position++];

            return new Token(result, TokenType.Name);
        }

        private Token NextInteger(char digit)
        {
            string result = digit.ToString();

            while (this.position < this.length && char.IsDigit(this.text[this.position]))
                result += this.text[this.position++];

            if (this.position < this.length)
            {
                char next = this.text[this.position];

                if (next == '.')
                    return this.NextReal(result);

                if (!punctuations.Contains(next) && !char.IsWhiteSpace(next))
                    throw new ParserException("Syntax error");
            }

            return new Token(result, TokenType.Integer);
        }

        private Token NextReal(string integer)
        {
            string result = integer + ".";
            this.position++;

            while (this.position < this.length && char.IsDigit(this.text[this.position]))
                result += this.text[this.position++];

            if (this.position < this.length)
            {
                char next = this.text[this.position];

                if (!punctuations.Contains(next) && !char.IsWhiteSpace(next))
                    throw new ParserException("Syntax error");
            }

            return new Token(result, TokenType.Real);
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
