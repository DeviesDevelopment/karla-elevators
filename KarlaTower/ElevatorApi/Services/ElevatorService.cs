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
    
    private void Init()
    {
        for (var i = 0; i < 5; i++)
        {
            Elevators.Add(new Elevator
            {
                Id = i
            });
        }
    }
}