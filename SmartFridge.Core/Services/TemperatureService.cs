using SmartFridge.Core.Interfaces;
using SmartFridge.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.Core.Services
{
    public class TemperatureService : ITemperatureService
    {
        private readonly User _user;
        private readonly IRepository<User> _userRepository;

        public event EventHandler<double> TemperatureChanged;

        public TemperatureService(User user, IRepository<User> userRepository)
        {
            _user = user;
            _userRepository = userRepository;
        }

        public double GetCurrentTemperature() => _user.FridgeSettings.Temperature;

        public void SetTemperature(double temperature)
        {
            if (temperature < -10 || temperature > 10)
                throw new ArgumentException("The temperature must be from -10°C to +10°C");

            _user.FridgeSettings.Temperature = temperature;
            _userRepository.Update(_user);
            _userRepository.SaveChanges();

            TemperatureChanged?.Invoke(this, temperature);
        }

        public void IncreaseTemperature() => SetTemperature(_user.FridgeSettings.Temperature + 0.5);
        public void DecreaseTemperature() => SetTemperature(_user.FridgeSettings.Temperature - 0.5);
    }
}
