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
        lock (Lock)
        {
            if (IsMoving)
                return;

            Direction = floor > CurrentLevel ? Direction.Up : Direction.Down;
            TargetLevel = floor;
            
            Task.Run(async () =>
            {
                while (IsMoving && CurrentLevel != TargetLevel)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    Move();
                }

                Stop();
            });
        }
    }

    public void Stop()
    {
        lock (Lock)
        {
            Direction = Direction.None;
        }
    }

    public void Enter()
    {
        lock (Lock)
        {
            if (Occupants < MaxOccupants)
                Occupants++;
        }
    }
    
    public void Leave()
    {
        lock (Lock)
        {
            if (Occupants > 0)
                Occupants--;
        }
    }

    private void Move()
    {
        lock (Lock)
        {
            if (CurrentLevel == TargetLevel)
                return;
            
            CurrentLevel += CurrentLevel < TargetLevel ? 1 : -1;
        }
    }
}