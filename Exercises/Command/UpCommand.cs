namespace Exercises.Command;

public class UpCommand : ICommand
{
    public override void Invoke()
    {
        Console.Write("Up ");
        Program.y++;
    }

    public override void Undo()
    {
        Console.Write("Down ");
        Program.y--;
    }
}