public class ReservationRequestDto
{
    public int RoomId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int PeopleCount { get; set; }
}
