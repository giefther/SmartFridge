using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFridge.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public Product? GetProductById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            return _productRepository.GetById(id);
        }

        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name cannot be empty", nameof(product));

            if (product.Category == null)
                throw new ArgumentException("Product category cannot be null", nameof(product));

            if (product.ExpirationDate < DateTime.Now.Date)
                throw new ArgumentException("Expiration date cannot be in the past", nameof(product));

            _productRepository.Add(product);
            _productRepository.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (product.Id == Guid.Empty)
                throw new ArgumentException("Product ID cannot be empty", nameof(product));

            var existingProduct = _productRepository.GetById(product.Id);
            if (existingProduct == null)
                throw new InvalidOperationException($"Product with ID {product.Id} not found");

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name cannot be empty", nameof(product));

            if (product.Category == null)
                throw new ArgumentException("Product category cannot be null", nameof(product));

            if (product.ExpirationDate < DateTime.Now.Date)
                throw new ArgumentException("Expiration date cannot be in the past", nameof(product));

            _productRepository.Update(product);
            _productRepository.SaveChanges();
        }

        public void DeleteProduct(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var product = _productRepository.GetById(id);
            if (product == null)
                throw new InvalidOperationException($"Product with ID {id} not found");

            _productRepository.Delete(id);
            _productRepository.SaveChanges();
        }

        public IEnumerable<Product> GetExpiredProducts()
        {
            return _productRepository.GetAll()
                .Where(p => p.ExpirationDate.Date < DateTime.Now.Date)
                .OrderBy(p => p.ExpirationDate);
        }

        public IEnumerable<Product> GetExpiringSoonProducts(int days = 3)
        {
            if (days <= 0)
                throw new ArgumentException("Days must be greater than 0", nameof(days));

            var thresholdDate = DateTime.Now.Date.AddDays(days);
            return _productRepository.GetAll()
                .Where(p => p.ExpirationDate.Date >= DateTime.Now.Date &&
                           p.ExpirationDate.Date <= thresholdDate)
                .OrderBy(p => p.ExpirationDate);
        }
    }
}