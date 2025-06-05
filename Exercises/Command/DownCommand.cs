namespace Exercises.Command;

public class DownCommand : ICommand
{
    public override void Invoke()
    {
        Console.Write("Down ");
        Program.y--;
    }

    public override void Undo()
    {
        Console.Write("Up ");
        Program.y++;
    }
}