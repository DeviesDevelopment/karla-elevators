namespace ElevatorCli;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Running challenges...");
        await new Challenge1().Solve();
        await new Challenge2().Solve();
        await new Challenge3().Solve();
    }
}