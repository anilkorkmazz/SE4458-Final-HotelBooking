using System.Net.Http;
using System.Net.Http.Json;
using NotificationService.Models;

namespace NotificationService.Services
{
    public class RoomQueryService : IRoomQueryService
    {
        private readonly HttpClient _httpClient;

        public RoomQueryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RoomInfoDto>> GetRoomsAsync()
        {
            var rooms = await _httpClient.GetFromJsonAsync<List<RoomInfoDto>>("http://localhost:5035/api/v1/Room/reservation-summary");
            return rooms ?? new List<RoomInfoDto>();                                
        }
    }
}
