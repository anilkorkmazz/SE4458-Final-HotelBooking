using StackExchange.Redis;
using System.Text.Json;
using System.Threading.Tasks;
using HotelService.Models;
using HotelService.DTOs;

namespace HotelService.Caching
{
    public class RoomCacheService
    {
        private readonly IDatabase _database;

        public RoomCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        // Redis'ten Room nesnesi getir
        public async Task<RoomDto?> GetRoomAsync(int roomId)
        {
            string key = GetKey(roomId);
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty) return null;

            return JsonSerializer.Deserialize<RoomDto>(value!);
        }

        // Redis'e Room nesnesi kaydet
        public async Task SetRoomAsync(RoomDto room)
        {
            string key = GetKey(room.Id);
            string json = JsonSerializer.Serialize(room);
            await _database.StringSetAsync(key, json);
        }

        // Redis key formatı
        private string GetKey(int roomId) => $"room:{roomId}";
    }
}
