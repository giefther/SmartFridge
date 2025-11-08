using SmartFridge.Core.Models;

namespace SmartFridge.Core.Interfaces
{
    public interface IUserService
    {
        User? Authenticate(string username, string password);
        bool Register(User user, string password);
        bool UserExists(string username);
    }
}
