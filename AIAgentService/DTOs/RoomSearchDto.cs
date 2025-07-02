namespace AIAgentService.DTOs
{
    public class RoomSearchDto
    {
        public string Location { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PeopleCount { get; set; }
    }
}
