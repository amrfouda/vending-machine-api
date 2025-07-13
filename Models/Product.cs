public class Product
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Cost { get; set; }
    public int AmountAvailable { get; set; }
    public int SellerId { get; set; }
    public User? Seller { get; set; }
}