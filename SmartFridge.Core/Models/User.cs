namespace SmartFridge.Core.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
        public string ProductsFilePath => Path.Combine("products", $"{Id}-products.json");
    }
}
