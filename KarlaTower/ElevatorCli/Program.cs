namespace ElevatorCli;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Running challenges...");
        var challenges = new List<Challenge>
        {
            new Challenge1(),
            new Challenge2(),
            new Challenge3(),
            new Challenge4(),
            new Challenge5()
        };

        foreach (var challenge in challenges)
        {
            await challenge.RunSolution();
        }
    }
}
