namespace ElevatorCli;

public class Challenge3 : Challenge
{
    public override Task RunSolution()
    {
        PrintResult(nameof(Challenge3), string.Empty);
        return Task.CompletedTask;
    }
}