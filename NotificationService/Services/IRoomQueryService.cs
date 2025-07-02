using System.Collections.Generic;
using System.Threading.Tasks;
using NotificationService.Models;

namespace NotificationService.Services
{
    public interface IRoomQueryService
    {
        Task<List<RoomInfoDto>> GetRoomsAsync();
    }
}
