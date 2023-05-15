namespace KarlaTower.Models;

public class Elevator
{
    private object Lock { get; } = new ();
    public int Id { get; init; }
    public int CurrentLevel { get; private set; }
    public int TargetLevel { get; private set; }
    public Direction Direction { get; private set; }
    public bool IsMoving => Direction != Direction.None;
    public int Occupants { get; private set; }
    public int MaxOccupants { get; init; }
    public string CurrentSong { get; private set; } = string.Empty;

    public void Send(int floor)
    {
        // Lock and check if there is a task already
        if (IsMoving)
            return;

        Direction = floor > CurrentLevel ? Direction.Up : Direction.Down;
        TargetLevel = floor;

        Task.Run(Move);
    }

    public void Stop()
    {
        Direction = Direction.None;
    }

    public void Enter(int floor)
    {
        lock (Lock)
        {
            if (Occupants < MaxOccupants && floor == CurrentLevel && !IsMoving)
                Occupants++;
        }
    }
    
    public void Leave(int floor)
    {
        lock (Lock)
        {
            if (Occupants > 0 && floor == CurrentLevel && !IsMoving)
                Occupants--;
        }
    }

    private async Task Move()
    {
        while (IsMoving)
        {
            lock (Lock)
            {
                if (CurrentLevel != TargetLevel)
                    CurrentLevel += CurrentLevel < TargetLevel ? 1 : -1;
                else
                    Stop();
            }
            
            await Task.Delay(TimeSpan.FromSeconds(3));
        }
    }
}