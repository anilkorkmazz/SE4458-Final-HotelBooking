using HotelService.Models;
using HotelService.DTOs;

namespace HotelService.Repositories
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllAsync(int pageNumber, int pageSize);
        Task<Room?> GetByIdAsync(int id);
        Task<Room> AddAsync(Room room);
        Task<Room?> UpdateAsync(int id, Room room);
        Task<bool> DeleteAsync(int id);

        // Pagination için toplam kayıt sayısı
        Task<int> CountAsync();

        // 🔍 Hotel ile birlikte tüm odaları getir (Search için kullanılıyor)
        Task<List<Room>> GetAllWithHotelAsync();

        Task<Hotel?> GetHotelByIdAsync(int hotelId);

        Task<List<RoomReservationSummaryDto>> GetRoomReservationSummariesAsync();

        Task<List<Room>> GetByHotelIdAsync(int hotelId, int page, int pageSize);
        
        Task<int> CountByHotelIdAsync(int hotelId);
    }
}
