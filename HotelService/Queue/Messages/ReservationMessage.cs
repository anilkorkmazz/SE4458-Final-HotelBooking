namespace HotelService.Queue.Messages
{
    public class ReservationMessage
    {
        public int RoomId { get; set; }
        public int PeopleCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
