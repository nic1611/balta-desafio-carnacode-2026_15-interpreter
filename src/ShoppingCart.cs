namespace DesignPatternChallenge;

public class ShoppingCart
{
    public decimal TotalValue { get; set; }
    public int ItemQuantity { get; set; }
    public string CustomerCategory { get; set; }
    public bool IsFirstPurchase { get; set; }
}