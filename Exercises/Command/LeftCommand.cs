namespace Exercises.Command;

public class LeftCommand : ICommand
{
    public override void Invoke()
    {
        Console.Write("Left ");
        Program.x--;
    }

    public override void Undo()
    {
        Console.Write("Right ");
        Program.x++;
    }
}