using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using SmartFridge.Core.Services;
using SmartFridge.Data;

namespace SmartFridge.UI.WinForms.Composition
{
    public static class CompositionRoot
    {
        public static IUserService UserService { get; }
        public static IProductService ProductService { get; }

        static CompositionRoot()
        {
            var userRepository = DataStorage.UserRepository;
            var productRepository = DataStorage.ProductRepository;

            UserService = new UserService(userRepository);
            ProductService = new ProductService(productRepository);
        }
    }
}