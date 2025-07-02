namespace HotelService.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;
        public string Description { get; set; } = default!;
        
        public List<Room> Rooms { get; set; } = new();
    }
}
