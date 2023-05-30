using System.Text;
using System.Text.Json;
using KarlaTower;
using KarlaTower.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ElevatorApiTest;

public class ElevatorControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    
    public ElevatorControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
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
        var response = await client.PutAsync($"elevators/{id}/send", body);
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
        var client = _factory.CreateClient();
        var floors = new[] { floor, floor - 1, floor + 1 };

        foreach (var f in floors)
        {
            var body = new StringContent($"{f}", Encoding.UTF8, "application/json");
            await client.PutAsync($"elevators/{id}/send", body);
        }
        
        var response = await client.GetAsync($"elevators/{id}");
        var elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        Assert.Equal(floor, elevator.TargetFloor);

        while (elevator.IsMoving)
        {
            response = await client.GetAsync($"elevators/{id}");
            elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        
        Assert.Equal(floor, elevator.CurrentFloor);
    }
    
    [Theory]
    [InlineData(0, 3, 2)]
    public async Task TestInterrupt(int id, int floor1, int floor2)
    {
        var client = _factory.CreateClient();
        
        var body1 = new StringContent($"{floor1}", Encoding.UTF8, "application/json");
        await client.PutAsync($"elevators/{id}/send", body1);
        await client.PutAsync($"elevators/{id}/stop", null);
        
        var body2 = new StringContent($"{floor2}", Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"elevators/{id}/send", body2);
        var elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        Assert.Equal(floor2, elevator.TargetFloor);

        while (elevator.IsMoving)
        {
            response = await client.GetAsync($"elevators/{id}");
            elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        
        Assert.Equal(floor2, elevator.CurrentFloor);
    }

    [Theory]
    [InlineData(5)]
    public async Task TestOrder(int floor)
    {
        var client = _factory.CreateClient();
        
        var response1 = await client.GetAsync($"elevators/order/{floor}");
        var elevator1 = (await response1.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        
        var response2 = await client.GetAsync($"elevators/order/{floor}");
        var elevator2 = (await response2.Content.ReadAsStringAsync()).FromJson<ElevatorData>();

        Assert.Equal(elevator1.Id, elevator2.Id);
        Assert.Equal(floor, elevator1.TargetFloor);
        Assert.True(elevator1.IsMoving || elevator1.CurrentFloor == elevator1.TargetFloor);
    }
    
    [Theory]
    [InlineData(3)]
    public async Task TestStop(int id)
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync($"elevators/{id}");
        var elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        var body = new StringContent($"{elevator.CurrentFloor + 10}", Encoding.UTF8, "application/json");
        response = await client.PutAsync($"elevators/{id}/send", body);
        elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        Assert.True(elevator.IsMoving);
        
        response = await client.PutAsync($"elevators/{id}/stop", null);
        elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        Assert.False(elevator.IsMoving);
    }
    
    [Theory]
    [InlineData(0)]
    public async Task TestEnterAndLeave(int floor)
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync($"elevators/order/{floor}");
        var elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();

        while (elevator.CurrentFloor != floor)
        {
            response = await client.GetAsync($"elevators/{elevator.Id}");
            elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        }

        for (var i = 0; i < elevator.MaxOccupants * 2; i++)
        {
            var enterBody = new StringContent($"{floor}", Encoding.UTF8, "application/json");
            response = await client.PostAsync($"elevators/{elevator.Id}/enter", enterBody);
            elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        }
        
        Assert.Equal(elevator.MaxOccupants, elevator.Occupants);
        
        var leaveBody = new StringContent($"{floor}", Encoding.UTF8, "application/json");
        response = await client.PostAsync($"elevators/{elevator.Id}/leave", leaveBody);
        elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        Assert.Equal(elevator.MaxOccupants - 1, elevator.Occupants);
        
        var sendBody = new StringContent($"{elevator.CurrentFloor + 1}", Encoding.UTF8, "application/json");
        await client.PutAsync($"elevators/{elevator.Id}/send", sendBody);
        response = await client.PostAsync($"elevators/{elevator.Id}/leave", leaveBody);
        elevator = (await response.Content.ReadAsStringAsync()).FromJson<ElevatorData>();
        Assert.Equal(elevator.MaxOccupants - 1, elevator.Occupants);
    }
}

public static class JsonExtensions
{
    public static T FromJson<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json) 
               ?? throw new InvalidOperationException($"{nameof(JsonSerializer.Deserialize)} returned null for type {nameof(T)}.");
    }
}