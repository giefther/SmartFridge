namespace SmartFridge.Core.Interfaces
{
    public interface ITemperatureService
    {
        double GetCurrentTemperature();
        void SetTemperature(double temperature);
        void IncreaseTemperature();
        void DecreaseTemperature();
        event EventHandler<double> TemperatureChanged;
    }
}
