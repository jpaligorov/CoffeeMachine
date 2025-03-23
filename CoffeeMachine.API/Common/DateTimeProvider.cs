namespace CoffeeMachine.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime CurrentDateTimeNow => DateTime.UtcNow;
}