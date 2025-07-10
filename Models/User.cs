public enum Role { Seller, Buyer }

public class User {
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; } // Hash it
    public int Deposit { get; set; }
    public Role Role { get; set; }
}