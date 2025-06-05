namespace Exercises.ChainOfResponsibility;

internal class LineReader
{
    public static void ReadLines()
    {
        while (true)
        {
            try
            {
                TheGreatDivider.MaxIntDividedBy(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Caught:" + ex.Message);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Enter A Number");
            }
        }
    }
}
