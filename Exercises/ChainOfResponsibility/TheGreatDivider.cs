namespace Exercises.ChainOfResponsibility;

internal class TheGreatDivider
{
    public static void MaxIntDividedBy(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Nothing Entered", "number");
        Console.WriteLine(int.MaxValue / int.Parse(number));
    }
}
