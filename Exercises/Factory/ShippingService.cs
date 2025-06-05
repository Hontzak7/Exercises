namespace Exercises.Factory;

internal class ShippingService
{
    NotificationServiceProvider _serviceProvider;
    public ShippingService(NotificationServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ShipItem()
    {
        //code to ship the item
        _serviceProvider.GetUserNotifier(false).NotifyUser(1);
    }
}