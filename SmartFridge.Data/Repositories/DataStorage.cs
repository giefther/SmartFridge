using SmartFridge.Core.Models;
using SmartFridge.Data.Repositories;
using System.IO;

namespace SmartFridge.Data
{
    public static class DataStorage
    {
        private static readonly string BasePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFiles");
        // json с наборами продуктов сохраняется отдельно для каждого пользователя.
        // Все json`ы хранятся в products
        private static readonly string ProductsPath = Path.Combine(BasePath, "products");
        public static JsonRepository<User> UserRepository { get; private set; }

        static DataStorage()
        {
            if (!Directory.Exists(BasePath))
                Directory.CreateDirectory(BasePath);

            if (!Directory.Exists(ProductsPath))
                Directory.CreateDirectory(ProductsPath);

            UserRepository = new JsonRepository<User>(Path.Combine(BasePath, "users.json"));
        }

        /// <summary>
        /// Подчищает json-файлы продуктов, пользователей который не сущетсвует
        /// </summary>
        public static void CleanupOrphanedProductFiles()
        {
            var productFiles = Directory.GetFiles(ProductsPath, "*.json");
            var existingUserIds = UserRepository.GetAll().Select(u => u.Id).ToHashSet();

            foreach (var filePath in productFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                if (Guid.TryParse(fileName, out var userId) && !existingUserIds.Contains(userId))
                {
                    File.Delete(filePath); // Удаляем файл несуществующего пользователя
                }
            }
        }

        /// <summary>
        /// Создаёт репозиторий для конкретного пользователя для дальнейшей работы через него с Json-файлом
        /// </summary>
        public static JsonRepository<Product> CreateProductRepository(User user)
        {
            var filePath = Path.Combine(BasePath, user.ProductsFilePath);
            return new JsonRepository<Product>(filePath);
        }
    }
}
