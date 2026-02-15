using System;
using System.Collections.Generic;

namespace DesignPatternChallenge
{
    public interface IExpression
    {
        object Interpret(ShoppingCart cart);
    }

    // Terminal Expression: Constants
    public class ConstantExpression : IExpression
    {
        private readonly object _value;
        public ConstantExpression(object value) => _value = value;
        public object Interpret(ShoppingCart cart) => _value;
    }

    // Terminal Expression: Variables from ShoppingCart
    public class VariableExpression : IExpression
    {
        private readonly string _variableName;
        public VariableExpression(string variableName) => _variableName = variableName;

        public object Interpret(ShoppingCart cart)
        {
            return _variableName switch
            {
                "quantidade" => cart.ItemQuantity,
                "valor" => cart.TotalValue,
                "categoria" => cart.CustomerCategory,
                "primeiraCompra" => cart.IsFirstPurchase,
                _ => throw new ArgumentException($"Variável desconhecida: {_variableName}")
            };
        }
    }

    // Non-terminal Expression: Comparison (>, <, =, >=, <=)
    public class ComparisonExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly string _operator;
        private readonly IExpression _right;

        public ComparisonExpression(IExpression left, string op, IExpression right)
        {
            _left = left;
            _operator = op;
            _right = right;
        }

        public object Interpret(ShoppingCart cart)
        {
            var leftVal = _left.Interpret(cart);
            var rightVal = _right.Interpret(cart);

            if (leftVal is IComparable comparable)
            {
                int comparison = comparable.CompareTo(Convert.ChangeType(rightVal, leftVal.GetType()));
                return _operator switch
                {
                    ">" => comparison > 0,
                    "<" => comparison < 0,
                    "=" => comparison == 0,
                    ">=" => comparison >= 0,
                    "<=" => comparison <= 0,
                    _ => throw new ArgumentException($"Operador inválido: {_operator}")
                };
            }

            if (leftVal is bool b1 && rightVal is bool b2)
            {
                return _operator switch
                {
                    "=" => b1 == b2,
                    _ => throw new ArgumentException($"Operador '{_operator}' não suportado para booleanos")
                };
            }

            if (leftVal is string s1 && rightVal is string s2)
            {
                return _operator switch
                {
                    "=" => s1 == s2,
                    _ => throw new ArgumentException($"Operador '{_operator}' não suportado para strings")
                };
            }

            return false;
        }
    }

    // Non-terminal Expression: Logical (AND, OR)
    public class LogicalExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly string _operator;
        private readonly IExpression _right;

        public LogicalExpression(IExpression left, string op, IExpression right)
        {
            _left = left;
            _operator = op;
            _right = right;
        }

        public object Interpret(ShoppingCart cart)
        {
            var leftResult = (bool)_left.Interpret(cart);

            // Operadores com curto-circuito
            if (_operator == "E") return leftResult && (bool)_right.Interpret(cart);
            if (_operator == "OU") return leftResult || (bool)_right.Interpret(cart);

            throw new ArgumentException($"Operador lógico inválido: {_operator}");
        }
    }

    // Non-terminal Expression: NOT
    public class NotExpression : IExpression
    {
        private readonly IExpression _expression;
        public NotExpression(IExpression expression) => _expression = expression;
        public object Interpret(ShoppingCart cart) => !(bool)_expression.Interpret(cart);
    }

    // Discount Rule: Condition + Percentage
    public class DiscountRule
    {
        public IExpression Condition { get; }
        public decimal DiscountPercentage { get; }
        public string RuleText { get; }

        public DiscountRule(IExpression condition, decimal discount, string text)
        {
            Condition = condition;
            DiscountPercentage = discount;
            RuleText = text;
        }

        public decimal Evaluate(ShoppingCart cart)
        {
            return (bool)Condition.Interpret(cart) ? DiscountPercentage : 0;
        }
    }
}