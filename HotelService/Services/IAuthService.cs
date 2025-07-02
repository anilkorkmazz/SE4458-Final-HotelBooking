using HotelService.DTOs;
using HotelService.Models;

namespace HotelService.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(UserLoginDto dto);
        Task<bool> RegisterAsync(UserRegisterDto dto);
        Task<User?> AuthenticateUserAsync(string username, string password); // ✅ Eklenen
        string GenerateJwtToken(User user); // ✅ Eklenen
    }
}
