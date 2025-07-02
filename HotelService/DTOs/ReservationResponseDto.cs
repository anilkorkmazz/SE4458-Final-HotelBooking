namespace HotelService.DTOs
{
    public class ReservationResponseDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Username { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PeopleCount { get; set; }  // 👈 EKLENDİ
    }
}
