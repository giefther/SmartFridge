namespace SmartFridge.Core.Models
{
    public class FridgeSettings
    {
        private double _temperature = 4.0;
        public double Temperature 
        {
            get => _temperature;
            set 
            {
                if (value < -10 || value > +10) 
                    throw new ArgumentException("The temperature must be from -10°C to +10°C");
                _temperature = value;
            }
        }
    }
}
