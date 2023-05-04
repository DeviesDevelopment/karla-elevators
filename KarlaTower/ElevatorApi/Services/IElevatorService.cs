using KarlaTower.Models;

namespace KarlaTower.Services;

public interface IElevatorService
{
    IEnumerable<Elevator> GetAllElevators();
    Elevator? GetElevator(int id);
}