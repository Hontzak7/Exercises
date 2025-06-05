namespace Exercises.ListResponsibilityChain;

internal class FileWritingItemHandler : IItemHandler
{
    public string fileName = DateTime.Now.ToFileTime().ToString();
    public IItemHandler Next { get; set; }
    public void Handle(string line)
    {
        File.AppendAllText(fileName, line + Environment.NewLine);
        Next?.Handle(line);
    }
}