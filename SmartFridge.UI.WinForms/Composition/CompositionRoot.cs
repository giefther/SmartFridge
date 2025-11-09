using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using SmartFridge.Core.Services;
using SmartFridge.Data;

namespace SmartFridge.UI.WinForms.Composition
{
    public static class CompositionRoot
    {
        public static IUserService UserService { get; }

        static CompositionRoot()
        {
            var userRepository = DataStorage.UserRepository;
            UserService = new UserService(userRepository);
        }

        // Фабричный метод для ProductService конкретного пользователя
        public static IProductService CreateProductService(User user)
        {
            var productRepository = DataStorage.CreateProductRepository(user);
            return new ProductService(productRepository);
        }
    }
}