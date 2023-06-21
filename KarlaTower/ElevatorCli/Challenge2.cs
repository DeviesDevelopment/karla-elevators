namespace ElevatorCli;

public class Challenge2 : Challenge
{
    protected override string GetName()
    {
        return "Challenge 2";
    }

    protected override Task<string> GetSolution()
    {
        return Task.FromResult(string.Empty);
    }
}