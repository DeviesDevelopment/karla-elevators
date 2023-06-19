namespace ElevatorCli;

public abstract class Challenge
{
    public abstract Task RunSolution();

    public void PrintResult(string challengeName, string result)
    {
        Console.WriteLine(string.IsNullOrEmpty(result)
            ? $"{challengeName} has no solution"
            : $"Solution to {challengeName} is: {result}");
    }
}