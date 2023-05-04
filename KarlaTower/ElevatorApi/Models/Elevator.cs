namespace KarlaTower.Models;

public class Elevator
{
    public int Id { get; init; }
    public int Level { get; set; }
    public Direction Direction { get; set; }
    public bool IsMoving { get; set; }
}