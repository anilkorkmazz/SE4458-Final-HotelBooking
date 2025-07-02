namespace NotificationService.Models
{
    public class RoomInfoDto
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int Capacity { get; set; }
        public int ReservedCount { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
    }
}
