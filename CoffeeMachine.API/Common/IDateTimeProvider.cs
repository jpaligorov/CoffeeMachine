namespace CoffeeMachine.Common;

public interface IDateTimeProvider
{
    DateTime CurrentDateTimeNow { get; }
}