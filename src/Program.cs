using System;
using System.Collections.Generic;

namespace DesignPatternChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Regras de Desconto com Interpreter ===\n");

            var carts = new List<ShoppingCart>
            {
                new ShoppingCart { TotalValue = 1500.00m, ItemQuantity = 15, CustomerCategory = "Regular", IsFirstPurchase = false },
                new ShoppingCart { TotalValue = 500.00m, ItemQuantity = 5, CustomerCategory = "VIP", IsFirstPurchase = false },
                new ShoppingCart { TotalValue = 200.00m, ItemQuantity = 2, CustomerCategory = "Regular", IsFirstPurchase = true }
            };

            var ruleStrings = new List<string>
            {
                "quantidade > 10 E valor > 1000 ENTAO 15",
                "categoria = \"VIP\" ENTAO 20",
                "primeiraCompra = true ENTAO 10"
            };

            // Novas regras complexas suportadas pelo Interpreter
            ruleStrings.Add("(quantidade > 10 OU valor > 500) E categoria = \"VIP\" ENTAO 25");
            ruleStrings.Add("NÃO primeiraCompra E quantidade >= 5 ENTAO 5");

            var rules = new List<DiscountRule>();
            foreach (var rs in ruleStrings)
            {
                try
                {
                    rules.Add(RuleParser.Parse(rs));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao parsear regra '{rs}': {ex.Message}");
                }
            }

            int cartId = 1;
            foreach (var cart in carts)
            {
                Console.WriteLine($"--- Carrinho {cartId++} ---");
                Console.WriteLine($"Qtd: {cart.ItemQuantity}, Valor: {cart.TotalValue:C}, Categoria: {cart.CustomerCategory}, 1ª Compra: {cart.IsFirstPurchase}");
                
                decimal maxDiscount = 0;
                foreach (var rule in rules)
                {
                    decimal discount = rule.Evaluate(cart);
                    if (discount > 0)
                    {
                        Console.WriteLine($"[APLICADA] {rule.RuleText} -> {discount}%");
                        if (discount > maxDiscount) maxDiscount = discount;
                    }
                    else
                    {
                        Console.WriteLine($"[NÃO APLICADA] {rule.RuleText}");
                    }
                }
                Console.WriteLine($"Desconto Final: {maxDiscount}%\n");
            }
        }
    }
}
