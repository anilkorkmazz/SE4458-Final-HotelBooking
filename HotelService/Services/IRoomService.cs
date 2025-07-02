using HotelService.DTOs;
using HotelService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelService.Services
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetAllRoomsAsync(int pageNumber, int pageSize);
 
        Task<PagedResult<RoomDto>> GetPaginatedRoomsAsync(int pageNumber, int pageSize);

        Task<RoomDto?> GetRoomByIdAsync(int id);

        Task<RoomDto> CreateRoomAsync(RoomDto dto);
        Task<Room?> UpdateRoomAsync(int id, RoomDto dto);
        Task<bool> DeleteRoomAsync(int id);

        Task<PagedResult<RoomDto>> GetPaginatedRoomsByHotelIdAsync(int hotelId, int page, int pageSize); // yeni ekledik 1 temmuz


        Task<PagedResult<RoomDto>> SearchAvailableRoomsAsync(RoomSearchDto dto, bool isLoggedIn, int page, int pageSize);
        Task<List<RoomReservationSummaryDto>> GetRoomReservationSummariesAsync();
    }
}
