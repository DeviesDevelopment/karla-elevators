using KarlaTower.Services;

namespace KarlaTower;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection collection)
    {
        return collection.AddSingleton<IElevatorService, ElevatorService>();
    }
}