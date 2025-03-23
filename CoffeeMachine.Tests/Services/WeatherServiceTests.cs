using System.Net;
using System.Text;
using System.Text.Json;
using CoffeeMachine.Common;
using CoffeeMachine.Configuration;
using CoffeeMachine.Features.BrewCoffee;
using CoffeeMachine.Models;
using CoffeeMachine.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace CoffeeMachine.Tests.Services;

public class WeatherServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly Mock<IOptions<WeatherApiConfig>> _configMock;

    public WeatherServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _configMock = new Mock<IOptions<WeatherApiConfig>>();

        _configMock.Setup(config => config.Value).Returns(new WeatherApiConfig
        {
            BaseUrl = "https://api.openweathermap.org/data/2.5/weather",
            ApiKey = "test-api-key"
        });
    }

    private HttpClient CreateHttpClient()
    {
        return new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.openweathermap.org")
        };
    }

    [Fact]
    public async Task GetCurrentTemperatureAsync_ShouldReturnTemperature_WhenResponseIsValid()
    {
        // Arrange
        var jsonResponse = new WeatherResponse
        {
            Main = new Main
            {
                Temp = 13.64
            }
        };

        var jsonString = JsonSerializer.Serialize(jsonResponse);


        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
            });

        var httpClient = CreateHttpClient();
        var service = new WeatherService(httpClient, _configMock.Object);

        // Act
        var result = await service.GetCurrentTemperatureAsync("Melbourne");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(13.64, result);
    }

    [Fact]
    public async Task Handle_ShouldReturnIcedCoffee_WhenTemperatureAbove30()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider
            .Setup(p => p.CurrentDateTimeNow)
            .Returns(new DateTime(2025, 3, 23));
        
        var mockWeatherService = new Mock<IWeatherService>();
        mockWeatherService
            .Setup(ws => ws.GetCurrentTemperatureAsync(It.IsAny<string>()))
            .ReturnsAsync(35);

        var handler = new BrewCoffeeHandler(mockDateTimeProvider.Object, mockWeatherService.Object);

        // Act
        var result = await handler.HandleRequestAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        var response = okResult?.Value as BrewCoffeeResponseDto;

        Assert.NotNull(response);
        Assert.Equal("Your refreshing iced coffee is ready", response?.Message);
    }

}