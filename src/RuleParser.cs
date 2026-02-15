using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternChallenge
{
    public class RuleParser
    {
        public static DiscountRule Parse(string ruleText)
        {
            // "quantidade > 10 E valor > 1000 ENTAO 15"
            var mainParts = ruleText.Split("ENTAO");
            if (mainParts.Length != 2)
                throw new ArgumentException("Formato de regra inválido. Deve conter 'ENTAO'.");

            string conditionText = mainParts[0].Trim();
            decimal discount = decimal.Parse(mainParts[1].Trim());

            IExpression condition = ParseCondition(conditionText);
            return new DiscountRule(condition, discount, ruleText);
        }

        private static IExpression ParseCondition(string text)
        {
            // Simple split by Logical operators (E, OU)
            // Note: This is a very basic parser and doesn't handle complex precedence/parentheses properly
            // but for the given examples it should work.
            
            if (text.Contains(" OU "))
            {
                var parts = text.Split(new[] { " OU " }, 2, StringSplitOptions.None);
                return new LogicalExpression(ParseCondition(parts[0]), "OU", ParseCondition(parts[1]));
            }

            if (text.Contains(" E "))
            {
                var parts = text.Split(new[] { " E " }, 2, StringSplitOptions.None);
                return new LogicalExpression(ParseCondition(parts[0]), "E", ParseCondition(parts[1]));
            }

            if (text.StartsWith("NÃO "))
            {
                return new NotExpression(ParseCondition(text.Substring(4)));
            }

            // Comparison: variable op value
            var comparisonOps = new[] { ">=", "<=", ">", "<", "=" };
            foreach (var op in comparisonOps)
            {
                if (text.Contains(op))
                {
                    var parts = text.Split(new[] { op }, StringSplitOptions.None);
                    var left = ParseOperand(parts[0].Trim());
                    var right = ParseOperand(parts[1].Trim());
                    return new ComparisonExpression(left, op, right);
                }
            }

            // If no operator, maybe it's just a boolean variable
            return ParseOperand(text);
        }

        private static IExpression ParseOperand(string text)
        {
            if (decimal.TryParse(text, out decimal d))
                return new ConstantExpression(d);
            
            if (bool.TryParse(text, out bool b))
                return new ConstantExpression(b);

            if (text.StartsWith("\"") && text.EndsWith("\""))
                return new ConstantExpression(text.Substring(1, text.Length - 2));

            // Default to variable
            return new VariableExpression(text);
        }
    }
}
