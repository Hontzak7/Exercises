namespace Exercises.ListResponsibilityChain;

internal class ConsoleItemHandler : IItemHandler
{
    public IItemHandler Next { get; set; }
    public void Handle(string line)
    {
        Console.WriteLine(line);
        Next?.Handle(line);
    }
}
