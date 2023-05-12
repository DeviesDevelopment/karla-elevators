using KarlaTower;
using KarlaTower.Controllers;
using KarlaTower.Models;
using KarlaTower.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorApiTest;

public class ElevatorControllerTest
{
    private readonly ElevatorsController _elevatorsController;
    
    public ElevatorControllerTest()
    {
        var services = new ServiceCollection();
        services.AddServices();
        var provider = services.BuildServiceProvider();

        var elevatorService = provider.GetService<IElevatorService>()!;
        _elevatorsController = new ElevatorsController(elevatorService);
    }
    
    [Fact]
    public void TestGet()
    {
        var response = _elevatorsController.Get().Result;
        var value = ParseOkResult<IEnumerable<Elevator>>(response);
        Assert.NotEmpty(value);
    }
    
    [Theory]
    [InlineData(0)]
    public void TestGetWithId(int id)
    {
        var response = _elevatorsController.Get(id).Result;
        var value = ParseOkResult<Elevator>(response);
        Assert.Equal(id, value.Id);
    }
    
    [Theory]
    [InlineData(0, 3)]
    public async Task TestSend(int id, int floor)
    {
        var response = _elevatorsController.SendElevator(id, floor).Result;
        var value = ParseOkResult<Elevator>(response);
        Assert.Equal(floor, value.TargetLevel);

        while (value.IsMoving)
        {
            value = ParseOkResult<Elevator>(_elevatorsController.Get(id).Result);
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        
        Assert.Equal(floor, value.CurrentLevel);
    }

    [Theory]
    [InlineData(5)]
    public void TestOrder(int floor)
    {
        var response1 = _elevatorsController.OrderElevator(floor).Result;
        var elevator1 = ParseOkResult<Elevator>(response1);
        var response2 = _elevatorsController.OrderElevator(floor).Result;
        var elevator2 = ParseOkResult<Elevator>(response2);
        Assert.Equal(elevator1.Id, elevator2.Id);
        Assert.Equal(floor, elevator1.TargetLevel);
        Assert.True(elevator1.IsMoving || elevator1.CurrentLevel == elevator1.TargetLevel);
    }
    
    [Theory]
    [InlineData(3)]
    public void TestStop(int id)
    {
        var response = _elevatorsController.Get(id).Result;
        var elevator = ParseOkResult<Elevator>(response);
        response = _elevatorsController.SendElevator(id, elevator.CurrentLevel + 10).Result;
        elevator = ParseOkResult<Elevator>(response);
        Assert.True(elevator.IsMoving);
        
        response = _elevatorsController.StopElevator(id).Result;
        elevator = ParseOkResult<Elevator>(response);
        Assert.False(elevator.IsMoving);
    }
    
    [Theory]
    [InlineData(0)]
    public void TestEnterAndLeave(int floor)
    {
        var response = _elevatorsController.OrderElevator(floor).Result;
        var elevator = ParseOkResult<Elevator>(response);

        while (elevator.CurrentLevel != floor)
        {
            response = _elevatorsController.Get(elevator.Id).Result;
            elevator = ParseOkResult<Elevator>(response);
        }

        for (var i = 0; i < elevator.MaxOccupants * 2; i++)
        {
            response = _elevatorsController.EnterElevator(elevator.Id, floor).Result;
            elevator = ParseOkResult<Elevator>(response);
        }
        
        Assert.Equal(elevator.MaxOccupants, elevator.Occupants);
        
        response = _elevatorsController.LeaveElevator(elevator.Id, floor).Result;
        elevator = ParseOkResult<Elevator>(response);
        Assert.Equal(elevator.MaxOccupants - 1, elevator.Occupants);
        
        _elevatorsController.SendElevator(elevator.Id, elevator.CurrentLevel + 1);
        response = _elevatorsController.EnterElevator(elevator.Id, floor).Result;
        elevator = ParseOkResult<Elevator>(response);
        Assert.Equal(elevator.MaxOccupants - 1, elevator.Occupants);
    }
    
    private static T ParseOkResult<T>(ActionResult? result) where T : class
    {
        Assert.NotNull(result);
        var objectResult = result as OkObjectResult;
        Assert.NotNull(objectResult);
        var response = objectResult!.Value as T;
        Assert.NotNull(response);
        return response!;
    }
}