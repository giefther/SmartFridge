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
            return _productRepository.GetById(id);
        }

        public void AddProduct(Product product)
        {
            _productRepository.Add(product);
            _productRepository.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.Update(product);
            _productRepository.SaveChanges();
        }

        public void DeleteProduct(Guid id)
        {
            _productRepository.Delete(id);
            _productRepository.SaveChanges();
        }

        public IEnumerable<Product> GetExpiredProducts()
        {
            return _productRepository.GetAll()
                .Where(p => p.ExpirationDate < DateTime.Now)
                .OrderBy(p => p.ExpirationDate);
        }

        public IEnumerable<Product> GetExpiringSoonProducts(int days)
        {
            var thresholdDate = DateTime.Now.AddDays(days);
            return _productRepository.GetAll()
                .Where(p => p.ExpirationDate >= DateTime.Now &&
                           p.ExpirationDate <= thresholdDate)
                .OrderBy(p => p.ExpirationDate);
        }
    }
}