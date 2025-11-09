using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using SmartFridge.Core.Utils;

namespace SmartFridge.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User? Authenticate(string username, string password)
        {
            var user = _userRepository.GetAll()
                .FirstOrDefault(u => u.Username == username);

            if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordHash))
                return null;

            return user;
        }

        public bool Register(User user, string password)
        {
            if (UserExists(user.Username))
                return false;

            user.PasswordHash = PasswordHasher.HashPassword(password);
            _userRepository.Add(user);
            _userRepository.SaveChanges();

            return true;
        }

        public bool UserExists(string username)
        {
            return _userRepository.GetAll()
                .Any(u => u.Username == username);
        }
    }
}