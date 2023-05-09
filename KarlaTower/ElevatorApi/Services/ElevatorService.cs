using KarlaTower.Models;

namespace KarlaTower.Services;

public class ElevatorService : IElevatorService
{
    private List<Elevator> Elevators { get; set; } = new();

    public ElevatorService()
    {
        Init();
    }

    public IEnumerable<Elevator> GetAllElevators()
    {
        return Elevators;
    }

    public Elevator? GetElevator(int id)
    {
        return Elevators.SingleOrDefault(e => e.Id == id);
    }

    public int OrderElevator(int floor)
    {
        throw new NotImplementedException();
    }

    public void EnterElevator(int id, int floor)
    {
        throw new NotImplementedException();
    }

    public void SendElevator(int id, int floor)
    {
        var elevator = Elevators.SingleOrDefault(e => e.Id == id) 
                       ?? throw new InvalidOperationException();
        elevator.Send(floor);
    }

    public void StopElevator(int id)
    {
        var elevator = Elevators.SingleOrDefault(e => e.Id == id) 
                       ?? throw new InvalidOperationException();
        elevator.Stop();
    }

    private void Init()
    {
        for (var i = 0; i < 5; i++)
        {
            Elevators.Add(new Elevator
            {
                Id = i,
                MaxOccupants = new Random().Next(2, 10)
            });
        }
    }
}