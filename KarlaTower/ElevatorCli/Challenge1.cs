namespace ElevatorCli;

public class Challenge1 : Challenge
{
    protected override string GetName()
    {
        return "Challenge 1";
    }

    protected override Task<string> GetSolution()
    {
        return Task.FromResult(string.Empty);
    }
}