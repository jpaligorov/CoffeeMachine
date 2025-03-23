using System.Text.Json;
using CoffeeMachine.Configuration;
using CoffeeMachine.Models;
using Microsoft.Extensions.Options;

namespace CoffeeMachine.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;

    private readonly string _baseWeatherUrl;
    private readonly string _apiKey;
    
    public WeatherService(HttpClient httpClient, IOptions<WeatherApiConfig> config)
    {
        _httpClient = httpClient;
        _baseWeatherUrl = config.Value.BaseUrl;
        _apiKey = config.Value.ApiKey;
    }
    
    public async Task<double?> GetCurrentTemperatureAsync(string city)
    {
        var requestUrl = $"{_baseWeatherUrl}?q={city}&appid={_apiKey}&units=metric";
        
        var response = await _httpClient.GetAsync(requestUrl);
        
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        // Parse the JSON response
        var weatherData = await JsonSerializer.DeserializeAsync<WeatherResponse>(
            await response.Content.ReadAsStreamAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        return weatherData?.Main.Temp;
    }
}