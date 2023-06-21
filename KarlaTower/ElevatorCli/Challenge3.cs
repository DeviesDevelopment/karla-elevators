namespace ElevatorCli;

public class Challenge3 : Challenge
{
    protected override string GetName()
    {
        return "Challenge 3";
    }

    protected override Task<string> GetSolution()
    {
        return Task.FromResult(string.Empty);
    }
}