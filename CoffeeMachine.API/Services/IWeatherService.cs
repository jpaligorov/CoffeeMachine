namespace CoffeeMachine.Services;

public interface IWeatherService
{
    Task<double?> GetCurrentTemperatureAsync(string city);
}