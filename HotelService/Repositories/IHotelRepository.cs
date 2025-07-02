using HotelService.Models;

namespace HotelService.Repositories
{
    public interface IHotelRepository
    {
        //Task<List<Hotel>> GetAllAsync();

        Task<List<Hotel>> GetAllAsync(string? location); // ← ekle


        Task<Hotel?> GetByIdAsync(int id);
        Task<Hotel> AddAsync(Hotel hotel);
        Task<Hotel?> UpdateAsync(int id, Hotel hotel);
        Task<bool> DeleteAsync(int id);

        
        Task<List<Hotel>> GetPagedAsync(int page, int pageSize);
        Task<int> GetTotalCountAsync();
    }
}
