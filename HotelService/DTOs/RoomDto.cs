using System.ComponentModel.DataAnnotations;

namespace HotelService.DTOs
{
    public class RoomDto
    {

        public int Id { get; set; } 

        [Required]
        public string RoomNumber { get; set; } = default!;
        [Required]
        public int Capacity { get; set; }
        [Required]
        public decimal PricePerNight { get; set; }
        [Required]
        public DateTime AvailableFrom { get; set; }
        [Required]
        public DateTime AvailableTo { get; set; }

        
        [Required]
        public int HotelId { get; set; }

        public HotelDto? Hotel { get; set; }

    }
}
