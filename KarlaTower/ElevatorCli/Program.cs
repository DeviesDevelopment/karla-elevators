namespace ElevatorCli;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Running challenges...");
        await new Challenge1().Run();
        await new Challenge2().Run();
        await new Challenge3().Run();
    }
}