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

    public Elevator? OrderElevator(int floor)
    {
        var elevator = Elevators
            .Where(e => 
                !e.IsMoving
                || floor >= e.CurrentLevel && e.Direction == Direction.Up
                || floor <= e.CurrentLevel && e.Direction == Direction.Down)
            .MinBy(e => Math.Abs(e.CurrentLevel - floor));

        if (elevator == null)
            return null;

        if (elevator.TargetLevel != floor)
        {
            elevator.Stop();
            elevator.Send(floor);
        }
        
        return elevator;
    }

    public Elevator? EnterElevator(int id, int floor)
    {
        var elevator = Elevators.SingleOrDefault(e => e.Id == id);
        if (elevator == null)
            return elevator;

        if (floor == elevator.CurrentLevel && !elevator.IsMoving)
        {
            elevator.Enter();
        }

        return elevator;
    }
    
    public Elevator? LeaveElevator(int id, int floor)
    {
        var elevator = Elevators.SingleOrDefault(e => e.Id == id);
        if (elevator == null)
            return elevator;

        if (floor == elevator.CurrentLevel && !elevator.IsMoving)
        {
            elevator.Leave();
        }

        return elevator;
    }

    public Elevator? SendElevator(int id, int floor)
    {
        var elevator = Elevators.SingleOrDefault(e => e.Id == id);
        if (elevator == null)
            return elevator;

        elevator.Send(floor);
        return elevator;
    }

    public Elevator? StopElevator(int id)
    {
        var elevator = Elevators.SingleOrDefault(e => e.Id == id);
        if (elevator == null)
            return elevator;
        
        elevator.Stop();
        return elevator;
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