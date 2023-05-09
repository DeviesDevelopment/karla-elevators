namespace KarlaTower.Models;

public class Elevator
{
    private object Lock { get; } = new ();
    public int Id { get; init; }
    public int Level { get; private set; }
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
                Direction = floor > Level ? Direction.Up : Direction.Down;
                
                while (floor != Level && IsMoving)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    Level += floor > Level ? 1 : -1;
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
}