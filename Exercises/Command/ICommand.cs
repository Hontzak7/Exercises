namespace Exercises.Command;

public abstract class ICommand
{
    public abstract void Invoke();
    public abstract void Undo();
}
