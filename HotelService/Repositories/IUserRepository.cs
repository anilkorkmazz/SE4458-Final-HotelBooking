using HotelService.Models;

namespace HotelService.Repositories
{
   public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User> AddUserAsync(User user);
    }
}



