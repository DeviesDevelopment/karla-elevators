using System.Text;
using System.Text.Json;
using KarlaTower;
using KarlaTower.Controllers;
using KarlaTower.Models;
using KarlaTower.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorApiTest;

public class ElevatorControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly ElevatorsController _elevatorsController;
    private readonly WebApplicationFactory<Program> _factory;
    
    public ElevatorControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        var services = new ServiceCollection();
        services.AddServices();
        var provider = services.BuildServiceProvider();

        var elevatorService = provider.GetService<IElevatorService>()!;
        _elevatorsController = new ElevatorsController(elevatorService);
    }
    
    [Fact]
    public async Task TestGet()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("elevators");
        response.EnsureSuccessStatusCode();
        var elevators = (await response.Content.ReadAsStringAsync()).FromJson<IEnumerable<ElevatorData>>();
        
        Assert.True(elevators.Any());
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task TestGetWithId(int id)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"elevators/{id}");
        response.EnsureSuccessStatusCode();
        var elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        
        Assert.Equal(id, elevator.Id);
    }
    
    [Theory]
    [InlineData(0, 3)]
    public async Task TestSend(int id, int floor)
    {
        var client = _factory.CreateClient();
        var body = new StringContent($"{floor}", Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"elevators/{id}/send", body);
        response.EnsureSuccessStatusCode();
        var elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        
        Assert.Equal(floor, elevator.TargetFloor);

        while (elevator.IsMoving)
        {
            response = await client.GetAsync($"elevators/{id}");
            response.EnsureSuccessStatusCode();
            elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        
        Assert.Equal(floor, elevator.CurrentFloor);
    }
    
    [Theory]
    [InlineData(0, 3)]
    public async Task TestConcurrentSend(int id, int floor)
    {
        _elevatorsController.SendElevator(id, floor);
        _elevatorsController.SendElevator(id, floor + 1);
        _elevatorsController.SendElevator(id, floor - 1);
        var response = _elevatorsController.Get(id).Result;
        var value = ParseOkResult<Elevator>(response);
        Assert.Equal(floor, value.TargetFloor);

        while (value.IsMoving)
        {
            value = ParseOkResult<Elevator>(_elevatorsController.Get(id).Result);
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        
        Assert.Equal(floor, value.CurrentFloor);
    }
    
    [Theory]
    [InlineData(0, 3, 2)]
    public async Task TestInterrupt(int id, int floor1, int floor2)
    {
        _elevatorsController.SendElevator(id, floor1);
        _elevatorsController.StopElevator(id);
        var response = _elevatorsController.SendElevator(id, floor2).Result;
        var elevator = ParseOkResult<Elevator>(response);
        Assert.Equal(floor2, elevator.TargetFloor);

        while (elevator.IsMoving)
        {
            elevator = ParseOkResult<Elevator>(_elevatorsController.Get(id).Result);
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        
        Assert.Equal(floor2, elevator.CurrentFloor);
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
        Assert.Equal(floor, elevator1.TargetFloor);
        Assert.True(elevator1.IsMoving || elevator1.CurrentFloor == elevator1.TargetFloor);
    }
    
    [Theory]
    [InlineData(3)]
    public void TestStop(int id)
    {
        var response = _elevatorsController.Get(id).Result;
        var elevator = ParseOkResult<Elevator>(response);
        response = _elevatorsController.SendElevator(id, elevator.CurrentFloor + 10).Result;
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

        while (elevator.CurrentFloor != floor)
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
        
        _elevatorsController.SendElevator(elevator.Id, elevator.CurrentFloor + 1);
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

public static class JsonExtensions
{
    public static T FromJson<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json) 
               ?? throw new InvalidOperationException($"{nameof(JsonSerializer.Deserialize)} returned null for type {nameof(T)}.");
    }
    
    public static string ToJson(this object value)
    {
        return JsonSerializer.Serialize(value);
    }
}