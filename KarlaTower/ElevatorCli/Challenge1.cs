namespace ElevatorCli;

public class Challenge1 : Challenge
{
    public override Task RunSolution()
    {
        PrintResult(nameof(Challenge1), string.Empty);
        return Task.CompletedTask;
    }
}