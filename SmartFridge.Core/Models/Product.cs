namespace SmartFridge.Core.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public Category Category { get; set; } = null!;
        public string CategoryName => Category?.Name;
        public DateTime ExpirationDate { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; } = "pcs";
        public DateTime AddedDate { get; set; } = DateTime.Now;
    }
}
