using HotelService.DTOs;
using HotelService.Models;

namespace HotelService.Services
{
    public interface IHotelService
    {
        //Task<List<Hotel>> GetAllHotelsAsync();
        Task<List<HotelDto>> GetAllHotelsAsync(string? location = null);


        Task<Hotel?> GetHotelByIdAsync(int id);
        Task<Hotel> CreateHotelAsync(HotelDto dto);
        Task<Hotel?> UpdateHotelAsync(int id, HotelDto dto);
        Task<bool> DeleteHotelAsync(int id);

        // ✅ Sayfalama (pagination) metodu
        Task<PagedResult<Hotel>> GetPagedHotelsAsync(int page, int pageSize);
    }
}
