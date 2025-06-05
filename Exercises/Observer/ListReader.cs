namespace Exercises.Observer;

internal class ListReader
{
    public delegate void NewListItem(string listItem);
    public event NewListItem ListUpdated;
    public void ReadList()
    {
        while (true)
        {
            var listItem = Console.ReadLine();
            if (ListUpdated != null)
                ListUpdated.Invoke(listItem);
        }
    }
}
