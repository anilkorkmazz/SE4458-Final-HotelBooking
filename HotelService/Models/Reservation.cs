using System.ComponentModel.DataAnnotations;

namespace HotelService.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int PeopleCount { get; set; }  

        public Room Room { get; set; } = default!;
    }
}
