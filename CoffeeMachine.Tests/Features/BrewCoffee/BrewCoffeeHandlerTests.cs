using CoffeeMachine.ActionResults;
using CoffeeMachine.Common;
using CoffeeMachine.Features.BrewCoffee;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CoffeeMachine.Tests.Features.BrewCoffee;

public class BrewCoffeeHandlerTests
{
    [Fact]
    public void Handle_ShouldReturn200()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider
            .Setup(p => p.CurrentDateTimeNow)
            .Returns(new DateTime(2025, 3, 23));
        
        var handler = new BrewCoffeeHandler(mockDateTimeProvider.Object);
        
        // Act
        var result = handler.HandleRequest();
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public void Handle_ShouldReturn418OnAprilFoolsDay()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider
            .Setup(p => p.CurrentDateTimeNow)
            .Returns(new DateTime(2025, 4, 1));

        var handler = new BrewCoffeeHandler(mockDateTimeProvider.Object);

        // Act
        var result = handler.HandleRequest();

        // Assert
        Assert.IsType<StatusCodeResult>(result);
        var statusCodeResult = result as StatusCodeResult;
        Assert.Equal(418, statusCodeResult?.StatusCode);
    }

    [Fact]
    public void Handle_ShouldReturn503EveryFifthRequest()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider
            .Setup(p => p.CurrentDateTimeNow)
            .Returns(new DateTime(2025, 3, 23));

        var handler = new BrewCoffeeHandler(mockDateTimeProvider.Object);

        // Act
        handler.HandleRequest();
        handler.HandleRequest(); 
        handler.HandleRequest();
        handler.HandleRequest();
        var result = handler.HandleRequest();

        // Assert
        Assert.IsType<CustomStatusCodeResult>(result);

        var statusCodeResult = result as CustomStatusCodeResult;
        Assert.Equal(503, statusCodeResult?.StatusCode);
    }

}