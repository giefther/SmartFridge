using SmartFridge.Core.Models;
using SmartFridge.Data.Repositories;
using System.IO;

namespace SmartFridge.Data
{
    public static class DataStorage
    {
        private static readonly string BasePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFiles");
        public static JsonRepository<Product> ProductRepository { get; private set; }
        public static JsonRepository<User> UserRepository { get; private set; }

        static DataStorage()
        {
            if (!Directory.Exists(BasePath))
                Directory.CreateDirectory(BasePath);

            ProductRepository = new JsonRepository<Product>(Path.Combine(BasePath, "products.json"));
            UserRepository = new JsonRepository<User>(Path.Combine(BasePath, "users.json"));
        }
    }
}
