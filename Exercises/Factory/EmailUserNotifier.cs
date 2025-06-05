namespace Exercises.Factory;

internal class EmailUserNotifier : IUserNotifier
{
    public void NotifyUser(int id)
    {
        Console.WriteLine($"Notified User {id} By Email");
    }
}
