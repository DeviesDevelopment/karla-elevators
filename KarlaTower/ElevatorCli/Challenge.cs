namespace ElevatorCli;

public abstract class Challenge
{
    protected abstract string GetName();
    protected abstract Task<string> GetSolution();
    
    public async Task Solve()
    {
        PrintResult(GetName(), await GetSolution());
    }

    private static void PrintResult(string challengeName, string result)
    {
        Console.WriteLine(string.IsNullOrEmpty(result)
            ? $"{challengeName} has no solution"
            : $"Solution to {challengeName} is: {result}");
    }
}