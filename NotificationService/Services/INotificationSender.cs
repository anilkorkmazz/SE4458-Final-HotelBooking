namespace NotificationService.Services
{
    public interface INotificationSender
    {
        Task SendAsync(string message);
    }
}
