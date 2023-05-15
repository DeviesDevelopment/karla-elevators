namespace KarlaTower.Models;

public class Elevator
{
    private object Lock { get; } = new ();
    private CancellationTokenSource? TokenSource { get; set; }
    
    public int Id { get; init; }
    public int CurrentFloor { get; private set; }
    public int TargetFloor { get; private set; }
    public Direction Direction { get; private set; }
    public bool IsMoving => Direction != Direction.None;
    public int Occupants { get; private set; }
    public int MaxOccupants { get; init; }
    public string CurrentSong { get; private set; } = string.Empty;

    public void Send(int floor)
    {
        lock (Lock)
        {
            if (TokenSource != null)
                return;
            
            Direction = floor > CurrentFloor ? Direction.Up : Direction.Down;
            TargetFloor = floor;
            TokenSource = new CancellationTokenSource();

            Console.WriteLine($"Sending elevator {Id} to floor {TargetFloor}");
            Task.Run(() => Move(TokenSource.Token));
        }
    }

    public void Stop()
    {
        Direction = Direction.None;
        TokenSource?.Cancel();
        TokenSource = null;
        Console.WriteLine($"Stopped elevator {Id}");
    }

    public void Enter(int floor)
    {
        lock (Lock)
        {
            if (Occupants < MaxOccupants && floor == CurrentFloor && !IsMoving)
            {
                Occupants++;
                Console.WriteLine($"Enter elevator {Id}, {Occupants} occupants");
            }
        }
    }
    
    public void Leave(int floor)
    {
        lock (Lock)
        {
            if (Occupants > 0 && floor == CurrentFloor && !IsMoving)
            {
                Occupants--;
                Console.WriteLine($"Leaving elevator {Id}, {Occupants} occupants");
            }
        }
    }

    private async Task Move(CancellationToken cancellationToken)
    {
        while (IsMoving && !cancellationToken.IsCancellationRequested)
        {
            lock (Lock)
            {
                if (CurrentFloor != TargetFloor)
                    CurrentFloor += CurrentFloor < TargetFloor ? 1 : -1;
                else
                    Stop();
            }
            
            await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
        }
    }
}