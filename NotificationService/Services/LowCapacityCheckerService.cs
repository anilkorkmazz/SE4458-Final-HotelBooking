using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using NotificationService.Services;

namespace NotificationService.Services
{
    public class LowCapacityCheckerService : BackgroundService
    {
        private readonly ILogger<LowCapacityCheckerService> _logger;
        private readonly IRoomQueryService _roomQueryService;
        private readonly INotificationSender _notificationSender;
        private readonly IEmailSender _emailSender;

        public LowCapacityCheckerService(
            ILogger<LowCapacityCheckerService> logger,
            IRoomQueryService roomQueryService,
            INotificationSender notificationSender,
            IEmailSender emailSender) 
        {
            _logger = logger;
            _roomQueryService = roomQueryService;
            _notificationSender = notificationSender;
            _emailSender = emailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("⏰ Low capacity kontrolü başlatıldı.");
                
                var rooms = await _roomQueryService.GetRoomsAsync();
                var lowCapacityRooms = rooms.Where(r =>
                    r.Capacity > 0 &&
                    r.ReservedCount >= r.Capacity * 0.8).ToList();

                foreach (var room in lowCapacityRooms)
                {
                    var message = $"⚠️ Uyarı: {room.HotelName} otelindeki {room.RoomNumber} numaralı odanın kapasitesi %20'nin altına düştü.";
                    await _notificationSender.SendAsync(message);
                    await _emailSender.SendEmailAsync("aniltennis11@gmail.com", "⚠️ Kapasite Uyarısı", message);
                }

                _logger.LogInformation("✅ Bildirimler ve e-postalar gönderildi.");

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Her 5 dakikada bir çalışır
            }
        }
    }
}
