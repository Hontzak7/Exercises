namespace Exercises.Factory;

internal class NotificationServiceProvider
{
    public IUserNotifier GetUserNotifier(bool inTesting = true)
    {
        if (inTesting)
        {
            return new TestUserNotifier();
        }
        else
        {
            return new EmailUserNotifier();
        }
    }
}