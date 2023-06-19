namespace ElevatorCli;

public class Challenge2 : Challenge
{
    public override Task RunSolution()
    {
        PrintResult(nameof(Challenge2), string.Empty);
        return Task.CompletedTask;
    }
}