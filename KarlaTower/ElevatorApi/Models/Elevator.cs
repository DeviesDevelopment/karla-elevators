namespace KarlaTower.Models;

public class Elevator
{
    private object Lock { get; } = new ();
    public int Id { get; init; }
    public int CurrentLevel { get; private set; }
    public int TargetLevel { get; private set; }
    public Direction Direction { get; private set; }
    public bool IsMoving { get; private set; }
    public int Occupants { get; private set; }
    public int MaxOccupants { get; init; }
    public string CurrentSong { get; private set; } = string.Empty;

    public void Send(int floor)
    {
        lock (Lock)
        {
            if (IsMoving)
                return;

            Task.Run(async () =>
            {
                IsMoving = true;
                Direction = floor > CurrentLevel ? Direction.Up : Direction.Down;
                TargetLevel = floor;
                
                while (floor != CurrentLevel && IsMoving)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    CurrentLevel += floor > CurrentLevel ? 1 : -1;
                }

                Stop();
            });
        }
    }

    public void Stop()
    {
        lock (Lock)
        {
            IsMoving = false;
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
}