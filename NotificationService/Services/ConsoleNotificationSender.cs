using Microsoft.Extensions.Logging;

namespace NotificationService.Services
{
    public class ConsoleNotificationSender : INotificationSender
    {
        private readonly ILogger<ConsoleNotificationSender> _logger;

        public ConsoleNotificationSender(ILogger<ConsoleNotificationSender> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(string message)
        {
            _logger.LogWarning("[🔔 Bildirim] " + message);
            return Task.CompletedTask;
        }
    }
}
