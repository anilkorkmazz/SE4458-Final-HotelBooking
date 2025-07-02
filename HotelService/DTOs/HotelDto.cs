namespace HotelService.DTOs
{
    public class HotelDto
    {

        public int Id { get; set; }
        
        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
