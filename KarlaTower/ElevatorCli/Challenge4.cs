namespace ElevatorCli;

public class Challenge4 : Challenge
{
    public override Task RunSolution()
    {
        PrintResult(nameof(Challenge4), string.Empty);
        return Task.CompletedTask;
    }
}