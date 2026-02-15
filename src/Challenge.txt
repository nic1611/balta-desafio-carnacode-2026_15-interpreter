// DESAFIO: Sistema de Regras de Desconto Dinâmicas
// PROBLEMA: Um e-commerce precisa avaliar regras complexas de desconto escritas em formato
// texto (ex: "Se quantidade > 10 E valor > 1000 ENTÃO desconto 15%"). O código atual
// usa muitos if/else e não permite que regras sejam configuradas dinamicamente

using System;
using System.Collections.Generic;

namespace DesignPatternChallenge
{
    public class ShoppingCart
    {
        public decimal TotalValue { get; set; }
        public int ItemQuantity { get; set; }
        public string CustomerCategory { get; set; }
        public bool IsFirstPurchase { get; set; }
    }

    // Problema: Regras de desconto hardcoded com condicionais complexas
    public class DiscountCalculator
    {
        public decimal CalculateDiscount(ShoppingCart cart, string ruleText)
        {
            // Problema: Parsing manual e limitado de regras
            // "quantidade>10 E valor>1000 ENTAO 15"
            // "categoria=VIP ENTAO 20"
            // "primeirCompra=true ENTAO 10"

            Console.WriteLine($"Avaliando regra: {ruleText}");

            // Tentativa ingênua de parsing
            if (ruleText.Contains("quantidade>10") && ruleText.Contains("valor>1000"))
            {
                if (cart.ItemQuantity > 10 && cart.TotalValue > 1000)
                {
                    // Extrair desconto do texto
                    var parts = ruleText.Split("ENTAO");
                    if (parts.Length > 1)
                    {
                        if (decimal.TryParse(parts[1].Trim(), out decimal discount))
                        {
                            Console.WriteLine($"✓ Regra aplicada: {discount}% desconto");
                            return discount;
                        }
                    }
                }
            }
            else if (ruleText.Contains("categoria=VIP"))
            {
                if (cart.CustomerCategory == "VIP")
                {
                    var parts = ruleText.Split("ENTAO");
                    if (parts.Length > 1 && decimal.TryParse(parts[1].Trim(), out decimal discount))
                    {
                        Console.WriteLine($"✓ Regra aplicada: {discount}% desconto");
                        return discount;
                    }
                }
            }
            else if (ruleText.Contains("primeiraCompra=true"))
            {
                if (cart.IsFirstPurchase)
                {
                    var parts = ruleText.Split("ENTAO");
                    if (parts.Length > 1 && decimal.TryParse(parts[1].Trim(), out decimal discount))
                    {
                        Console.WriteLine($"✓ Regra aplicada: {discount}% desconto");
                        return discount;
                    }
                }
            }

            Console.WriteLine("✗ Regra não aplicável");
            return 0;
        }

        // Problema: Adicionar nova regra = modificar código
        // Problema: Não suporta operadores complexos (OU, NÃO, parênteses)
        // Problema: Não valida sintaxe das regras
    }

    // Tentativa alternativa: Eval dinâmico (perigoso e limitado)
    public class DynamicRuleEvaluator
    {
        public bool EvaluateRule(ShoppingCart cart, string expression)
        {
            // Substituir variáveis
            expression = expression
                .Replace("quantidade", cart.ItemQuantity.ToString())
                .Replace("valor", cart.TotalValue.ToString())
                .Replace("categoria", $"\"{cart.CustomerCategory}\"")
                .Replace("primeiraCompra", cart.IsFirstPurchase.ToString().ToLower());

            Console.WriteLine($"Expressão transformada: {expression}");

            // Problema: Usar eval/compilar código dinamicamente é perigoso
            // Problema: Difícil validar e debugar
            // Problema: Performance ruim (compilação em runtime)

            // Não implementado aqui por questões de segurança
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Regras de Desconto ===\n");

            var calculator = new DiscountCalculator();

            var cart1 = new ShoppingCart
            {
                TotalValue = 1500.00m,
                ItemQuantity = 15,
                CustomerCategory = "Regular",
                IsFirstPurchase = false
            };

            var cart2 = new ShoppingCart
            {
                TotalValue = 500.00m,
                ItemQuantity = 5,
                CustomerCategory = "VIP",
                IsFirstPurchase = false
            };

            var cart3 = new ShoppingCart
            {
                TotalValue = 200.00m,
                ItemQuantity = 2,
                CustomerCategory = "Regular",
                IsFirstPurchase = true
            };

            // Regras definidas como strings (idealmente viriam de banco de dados)
            var rules = new List<string>
            {
                "quantidade>10 E valor>1000 ENTAO 15",
                "categoria=VIP ENTAO 20",
                "primeiraCompra=true ENTAO 10"
            };

            Console.WriteLine("=== Carrinho 1 ===");
            foreach (var rule in rules)
            {
                calculator.CalculateDiscount(cart1, rule);
            }

            Console.WriteLine("\n=== Carrinho 2 ===");
            foreach (var rule in rules)
            {
                calculator.CalculateDiscount(cart2, rule);
            }

            Console.WriteLine("\n=== Carrinho 3 ===");
            foreach (var rule in rules)
            {
                calculator.CalculateDiscount(cart3, rule);
            }

            Console.WriteLine("\n=== PROBLEMAS ===");
            Console.WriteLine("✗ Parsing manual limitado e frágil");
            Console.WriteLine("✗ Não suporta expressões complexas (parênteses, precedência)");
            Console.WriteLine("✗ Não suporta operadores lógicos compostos (E, OU, NÃO)");
            Console.WriteLine("✗ Adicionar nova operação requer modificar código");
            Console.WriteLine("✗ Difícil validar sintaxe das regras");
            Console.WriteLine("✗ Impossível criar DSL (Domain Specific Language) rica");
            Console.WriteLine("✗ Não há árvore de sintaxe para otimização");

            Console.WriteLine("\n=== Expressões Desejadas (não suportadas) ===");
            Console.WriteLine("• (quantidade > 10 OU valor > 500) E categoria = VIP");
            Console.WriteLine("• NÃO primeiraCompra E quantidade >= 5");
            Console.WriteLine("• (valor > 1000 E categoria = VIP) OU primeiraCompra");

            // Perguntas para reflexão:
            // - Como interpretar gramática de uma linguagem?
            // - Como representar expressões como árvore de sintaxe?
            // - Como avaliar expressões recursivamente?
            // - Como criar linguagem específica de domínio extensível?
        }
    }
}
