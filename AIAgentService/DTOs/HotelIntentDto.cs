namespace AIAgentService.DTOs
{
    public class HotelIntentDto
    {
        public string Intent { get; set; } = string.Empty;
        public string Username { get; set; } // 👈 yeni eklendi
        public string Location { get; set; } = string.Empty;
        public string HotelName { get; set; } = string.Empty; // ✅ Yeni eklendi
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PeopleCount { get; set; }
    }
}
