using System.ComponentModel.DataAnnotations;

namespace HotelService.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string PasswordHash { get; set; } = default!;

        public string Role { get; set; } = "User"; // opsiyonel, default "User"
    }
}
