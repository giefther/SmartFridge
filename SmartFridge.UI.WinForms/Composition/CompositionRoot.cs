using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using SmartFridge.Core.Services;
using SmartFridge.Data;
using System.Collections.Generic;

namespace SmartFridge.UI.WinForms.Composition
{
    public static class CompositionRoot
    {
        private static readonly Dictionary<Guid, IProductService> _productServices = new();

        public static IUserService UserService { get; }

        static CompositionRoot()
        {
            var userRepository = DataStorage.UserRepository;
            UserService = new UserService(userRepository);
        }

        /// <summary>
        /// Получает ProductService для конкретного пользователя с кэшированием
        /// </summary>
        public static IProductService GetProductService(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.Id == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty", nameof(user));

            // Возвращаем из кэша, если есть
            if (_productServices.TryGetValue(user.Id, out var cachedService))
                return cachedService;

            // Создаем новый сервис и кэшируем
            var productRepository = DataStorage.CreateProductRepository(user);
            var productService = new ProductService(productRepository);

            _productServices[user.Id] = productService;
            return productService;
        }

        public static ITemperatureService GetTemperatureService(User user)
        {
            var userRepository = DataStorage.UserRepository;
            return new TemperatureService(user, userRepository);
        }

        /// <summary>
        /// Очищает кэш ProductService для конкретного пользователя
        /// </summary>
        public static void ClearUserCache(Guid userId)
        {
            _productServices.Remove(userId);
        }

        /// <summary>
        /// Очищает весь кэш ProductService (например, при выходе из приложения)
        /// </summary>
        public static void ClearAllCache()
        {
            _productServices.Clear();
        }

        /// <summary>
        /// Получает количество закэшированных сервисов (для отладки)
        /// </summary>
        public static int GetCacheCount() => _productServices.Count;
    }
}