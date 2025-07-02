using System.ComponentModel.DataAnnotations;


namespace HotelService.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
