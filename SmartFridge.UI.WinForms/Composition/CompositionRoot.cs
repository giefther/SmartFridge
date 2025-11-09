using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using SmartFridge.Core.Services;
using SmartFridge.Data;

namespace SmartFridge.UI.WinForms.Composition
{
    /// <summary>
    /// Composition Root - место где создаются и связываются все зависимости приложения
    /// </summary>
    public static class CompositionRoot
    {
        public static IUserService UserService { get; }

        static CompositionRoot()
        {
            var userRepository = DataStorage.UserRepository;


            UserService = new UserService(userRepository);

            // В будущем здесь будут другие сервисы:
            // ProductService = new ProductService(DataStorage.ProductRepository);
        }
    }
}