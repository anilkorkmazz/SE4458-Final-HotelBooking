using NotificationService.Queue.Messages;

namespace NotificationService.Services
{
    public class NotificationHandler
    {
        private readonly ILogger<NotificationHandler> _logger;

        public NotificationHandler(ILogger<NotificationHandler> logger)
        {
            _logger = logger;
        }

        public Task ProcessReservationMessageAsync(ReservationMessage message)
        {
            // Bu örnekte sadece log yazıyoruz, gerçek projede e-posta/SMS gibi aksiyonlar alınabilir
            _logger.LogInformation("📩 New Reservation Received:");
            _logger.LogInformation($"RoomId: {message.RoomId}");
            _logger.LogInformation($"PeopleCount: {message.PeopleCount}");
            _logger.LogInformation($"StartDate: {message.StartDate}");
            _logger.LogInformation($"EndDate: {message.EndDate}");

            
            return Task.CompletedTask;
        }
    }
}
