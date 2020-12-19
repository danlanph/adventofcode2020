using System;
using System.Collections.Generic;
using System.Text;

namespace danl.adventofcode2020.OperationOrder18
{
    public class Scanner
    {
        private readonly string _expressionString;
        private int _start;
        private int _current;
        private bool _scanned = false;
        private IList<Token> _tokens = new List<Token>();

        public Scanner(string expressionString)
        {
            if (string.IsNullOrWhiteSpace(expressionString))
                throw new ArgumentNullException(nameof(expressionString));

            _expressionString = expressionString;
        }

        public IList<Token> GetTokens()
        {
            if (_scanned)
                return _tokens;

            _current = 0;
            while (!IsEndOfExpression())
            {
                _start = _current;
                char currentChar = _expressionString[_current++];

                if (Char.IsWhiteSpace(currentChar))
                    continue;

                if (Char.IsDigit(currentChar))
                {
                    while (!IsEndOfExpression() && Char.IsDigit(_expressionString[_current]))
                        _current++;

                    AddToken(TokenType.Number);

                    continue;
                }

                switch (currentChar)
                {
                    case '(':
                        AddToken(TokenType.LeftBracket);
                        break;
                    case ')':
                        AddToken(TokenType.RightBracket);
                        break;
                    case '+':
                        AddToken(TokenType.Plus);
                        break;
                    case '*':
                        AddToken(TokenType.Multiply);
                        break;
                }
            }

            _scanned = true;
            return _tokens;
        }

        private bool IsEndOfExpression()
        {
            return _current == _expressionString.Length;
        }

        private void AddToken(TokenType tokenType)
        {
            _tokens.Add(new Token { TokenType = tokenType, Lexeme = _expressionString.Substring(_start, _current - _start) });
        }
    }
}
