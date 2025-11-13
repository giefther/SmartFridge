using SmartFridge.Core.Models;

namespace SmartFridge.Core.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product? GetProductById(Guid id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Guid id);
        IEnumerable<Product> GetExpiredProducts();
        IEnumerable<Product> GetExpiringSoonProducts(int days);
    }
}
