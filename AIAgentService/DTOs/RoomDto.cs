public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Location { get; set; } = "";
    public string Description { get; set; } = "";
}

public class RoomDto
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = "";
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public DateTime AvailableFrom { get; set; }
    public DateTime AvailableTo { get; set; }
    public HotelDto Hotel { get; set; } = new();
}
