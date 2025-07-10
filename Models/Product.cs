public class Product {
    public int Id { get; set; }
    public string ProductName { get; set; }
    public int AmountAvailable { get; set; }
    public int Cost { get; set; } // Must be divisible by 5
    public int SellerId { get; set; }
    public User Seller { get; set; }
}