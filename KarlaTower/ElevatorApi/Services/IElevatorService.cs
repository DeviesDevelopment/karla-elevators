using KarlaTower.Models;

namespace KarlaTower.Services;

public interface IElevatorService
{
    IEnumerable<Elevator> GetAllElevators();
    Elevator? GetElevator(int id);
    Elevator? OrderElevator(int floor);
    Elevator? EnterElevator(int id,  int floor);
    Elevator? LeaveElevator(int id,  int floor);
    Elevator? SendElevator(int id,  int floor);
    Elevator? StopElevator(int id);
}