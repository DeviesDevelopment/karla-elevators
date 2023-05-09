using KarlaTower.Models;

namespace KarlaTower.Services;

public interface IElevatorService
{
    IEnumerable<Elevator> GetAllElevators();
    Elevator? GetElevator(int id);
    int OrderElevator(int floor);
    void EnterElevator(int id,  int floor);
    void SendElevator(int id,  int floor);
    void StopElevator(int id);
}