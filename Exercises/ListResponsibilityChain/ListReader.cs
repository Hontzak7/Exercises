namespace Exercises.ListResponsibilityChain;

internal class ListReader
{
    public ListReader AddHandler(IItemHandler newHandler)
    {
        if (FirstHandler == null)
            FirstHandler = LastHandler = newHandler;
        else
            LastHandler = LastHandler.Next = newHandler;
        return this;
    }

    IItemHandler FirstHandler;
    IItemHandler LastHandler;
    public void ReadList()
    {
        while (true)
        {
            var listItem = Console.ReadLine();
            FirstHandler?.Handle(listItem);
        }
    }
}
