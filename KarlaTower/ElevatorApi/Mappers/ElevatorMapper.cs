using KarlaTower.Models;

namespace KarlaTower.Mappers;

public static class ElevatorMapper
{
    public static ElevatorData ToElevatorData(this Elevator elevator)
    {
        return new ElevatorData
        {
            Direction = elevator.Direction,
            Id = elevator.Id,
            Occupants = elevator.Occupants,
            CurrentFloor = elevator.CurrentFloor,
            CurrentSong = elevator.CurrentSong,
            IsMoving = elevator.IsMoving,
            MaxOccupants = elevator.MaxOccupants,
            TargetFloor = elevator.TargetFloor
        };
    }
}