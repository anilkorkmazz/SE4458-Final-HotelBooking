using System.ComponentModel.DataAnnotations;

namespace HotelService.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Required]
        public string RoomNumber { get; set; } = default!;
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }

        
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = default!;

        public ICollection<Reservation> Reservations { get; set; }
    }
}
