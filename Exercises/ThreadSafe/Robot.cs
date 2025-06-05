namespace Exercises.ThreadSafe;

internal struct Robot
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Team { get; set; }
    public ConsoleColor TeamColor { get; set; }
    public int GemstoneCount { get; set; }
}
