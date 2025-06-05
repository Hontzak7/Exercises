namespace Exercises.Command;

public class RightCommand : ICommand
{
    public override void Invoke()
    {
        Console.Write("Right ");
        Program.x++;
    }
    public override void Undo()
    {
        Console.Write("Left ");
        Program.x--;
    }
}
