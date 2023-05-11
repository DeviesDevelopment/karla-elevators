using KarlaTower;
using KarlaTower.Controllers;
using KarlaTower.Models;
using KarlaTower.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorApiTest;

public class ElevatorControllerTest
{
    [Fact]
    public void TestGet()
    {
        var services = new ServiceCollection();
        services.AddServices();
        var provider = services.BuildServiceProvider();

        var elevatorService = provider.GetService<IElevatorService>()!;
        var controller = new ElevatorsController(elevatorService);

        var response = controller.Get().Result;
        Assert.IsType<OkObjectResult>(response);
        var value = (response as OkObjectResult)!.Value as IEnumerable<Elevator>;
        Assert.NotEmpty(value!);
    }
}