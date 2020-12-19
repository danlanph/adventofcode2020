using System;
using System.Collections.Generic;
using System.Text;

namespace danl.adventofcode2020.OperationOrder18
{
    public abstract class Expression
    {
        public abstract ulong Evaluate();
    }

    public class BinaryExpression : Expression
    {
        private readonly Expression _left;
        private readonly Expression _right;
        private readonly Token _operator;

        public BinaryExpression(Expression left, Token expressionOperator, Expression right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (expressionOperator == null)
                throw new ArgumentNullException(nameof(expressionOperator));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            _left = left;
            _right = right;
            _operator = expressionOperator;
        }

        public override ulong Evaluate()
        {
            var left = _left.Evaluate();
            var right = _right.Evaluate();

            if (_operator.TokenType == TokenType.Multiply)
                return left * right;

            return left + right;
        }
    }

    public class GroupingExpression : Expression
    {
        private readonly Expression _subExpression;

        public GroupingExpression(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            _subExpression = expression;
        }

        public override ulong Evaluate()
        {
            return _subExpression.Evaluate();
        }
    }

    public class NumberLiteralExpression : Expression
    {
        private readonly ulong _literal;

        public NumberLiteralExpression(string literal)
        {
            _literal = ulong.Parse(literal);
        }

        public override ulong Evaluate()
        {
            return _literal;
        }
    }
}
