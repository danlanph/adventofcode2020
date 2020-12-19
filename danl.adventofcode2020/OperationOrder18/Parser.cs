using System;
using System.Collections.Generic;
using System.Text;

namespace danl.adventofcode2020.OperationOrder18
{
    public class Parser
    {
        private readonly IList<Token> _tokens;
        private int _current;

        public Parser(IList<Token> tokens)
        {
            if (tokens == null || tokens.Count == 0)
                throw new ArgumentNullException(nameof(tokens));

            _tokens = tokens;
        }

        public Expression ParseWithEqualPrecedence()
        {
            _current = 0;
            return ExpressionNoPrecedence();
        }

        public Expression ParseWithAdditionHigherPrecedence()
        {
            _current = 0;
            return ExpressionWithPrecedence();
        }

        private Expression ExpressionNoPrecedence()
        {
            var expression = Unit(() => ExpressionNoPrecedence());

            while (Match(TokenType.Multiply, TokenType.Plus))
            {
                var binaryOperator = _tokens[_current - 1];
                var right = Unit(() => ExpressionNoPrecedence());

                expression = new BinaryExpression(expression, binaryOperator, right);
            }

            return expression;
        }

        private Expression ExpressionWithPrecedence()
        {
            var expression = Factor();

            while (Match(TokenType.Multiply))
            {
                var binaryOperator = _tokens[_current - 1];
                var right = Factor();

                expression = new BinaryExpression(expression, binaryOperator, right);
            }

            return expression;
        }

        private Expression Factor()
        {
            var expression = Unit(() => ExpressionWithPrecedence());

            while (Match(TokenType.Plus))
            {
                var binaryOperator = _tokens[_current - 1];
                var right = Unit(() => ExpressionWithPrecedence());

                expression = new BinaryExpression(expression, binaryOperator, right);
            }

            return expression;
        }

        private Expression Unit(Func<Expression> rootExpression)
        {
            if (Match(TokenType.LeftBracket))
            {
                var subExpression = rootExpression();
                if (!Match(TokenType.RightBracket))
                    throw new InvalidOperationException();

                return new GroupingExpression(subExpression);
            }

            if (!CheckCurrentType(TokenType.Number))
                throw new InvalidOperationException();

            return new NumberLiteralExpression(_tokens[_current++].Lexeme);
        }

        private bool Match(params TokenType[] tokenTypes)
        {
            foreach (var tokenType in tokenTypes)
            {
                if (CheckCurrentType(tokenType))
                {
                    _current++;
                    return true;
                }
            }

            return false;
        }

        private bool CheckCurrentType(TokenType tokenType)
        {
            return _current < _tokens.Count && _tokens[_current].TokenType == tokenType;
        }
    }
}
