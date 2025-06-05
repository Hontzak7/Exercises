namespace Exercises.Observer;

internal class FileWriter
{
    public string fileName = DateTime.Now.ToFileTime().ToString();
    public void WriteToFile(string line) => File.AppendAllText(fileName, line + Environment.NewLine);
}
