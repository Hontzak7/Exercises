namespace Exercises.Proxy;

internal static class OneFileAtATimeProxy
{
    static readonly object FileInUse = new object();

    public static void AppendAllText(string filename, string text)
    {
        lock (FileInUse)
        {
            File.AppendAllText(filename, text);
        }

    }
}
