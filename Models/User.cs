public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int Deposit { get; set; } = 0;
    public string Role { get; set; } = string.Empty; // buyer or seller
}