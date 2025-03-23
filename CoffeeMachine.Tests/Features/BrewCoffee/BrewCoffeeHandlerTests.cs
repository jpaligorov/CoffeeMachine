using CoffeeMachine.ActionResults;
using CoffeeMachine.Common;
using CoffeeMachine.Features.BrewCoffee;
using CoffeeMachine.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CoffeeMachine.Tests.Features.BrewCoffee;

public class BrewCoffeeHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturn200()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider
            .Setup(p => p.CurrentDateTimeNow)
            .Returns(new DateTime(2025, 3, 23));
        
        var mockWeatherService = new Mock<IWeatherService>();
        mockWeatherService
            .Setup(ws => ws.GetCurrentTemperatureAsync(It.IsAny<string>()))
            .ReturnsAsync(20);

        
        var handler = new BrewCoffeeHandler(mockDateTimeProvider.Object, mockWeatherService.Object);
        
        // Act
        var result = await handler.HandleRequestAsync();
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task Handle_ShouldReturn418OnAprilFoolsDay()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider
            .Setup(p => p.CurrentDateTimeNow)
            .Returns(new DateTime(2025, 4, 1));
        
        var mockWeatherService = new Mock<IWeatherService>();
        mockWeatherService
            .Setup(ws => ws.GetCurrentTemperatureAsync(It.IsAny<string>()))
            .ReturnsAsync(20);


        var handler = new BrewCoffeeHandler(mockDateTimeProvider.Object, mockWeatherService.Object);

        // Act
        var result = await handler.HandleRequestAsync();

        // Assert
        Assert.IsType<StatusCodeResult>(result);
        var statusCodeResult = result as StatusCodeResult;
        Assert.Equal(418, statusCodeResult?.StatusCode);
    }

    [Fact]
    public async Task Handle_ShouldReturn503EveryFifthRequest()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider
            .Setup(p => p.CurrentDateTimeNow)
            .Returns(new DateTime(2025, 3, 23));
        
        var mockWeatherService = new Mock<IWeatherService>();
        mockWeatherService
            .Setup(ws => ws.GetCurrentTemperatureAsync(It.IsAny<string>()))
            .ReturnsAsync(20);


        var handler = new BrewCoffeeHandler(mockDateTimeProvider.Object, mockWeatherService.Object);

        // Act
        handler.HandleRequestAsync();
        handler.HandleRequestAsync(); 
        handler.HandleRequestAsync();
        handler.HandleRequestAsync();
        var result = await handler.HandleRequestAsync();

        // Assert
        Assert.IsType<CustomStatusCodeResult>(result);

        var statusCodeResult = result as CustomStatusCodeResult;
        Assert.Equal(503, statusCodeResult?.StatusCode);
    }

}