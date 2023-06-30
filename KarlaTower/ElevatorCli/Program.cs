namespace ElevatorCli;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Running challenges...");
        new Challenge1().Run();
        new Challenge2().Run();
        new Challenge3().Run();
    }
}