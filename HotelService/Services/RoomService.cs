using HotelService.DTOs;
using HotelService.Models;
using HotelService.Repositories;
using HotelService.Caching; 

namespace HotelService.Services
{
    public class RoomService : IRoomService
    {
        private readonly ILogger<RoomService> _logger;
        private readonly IRoomRepository _repo;
        private readonly RoomCacheService _cache;
        
        public RoomService(IRoomRepository repo, RoomCacheService cache, ILogger<RoomService> logger)
        {
            _repo = repo;
            _cache = cache;
            _logger = logger;
        }


        public async Task<List<RoomDto>> GetAllRoomsAsync(int pageNumber, int pageSize)
        {
            var rooms = await _repo.GetAllAsync(pageNumber, pageSize);
            return rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                Capacity = r.Capacity,
                PricePerNight = r.PricePerNight,
                AvailableFrom = r.AvailableFrom,
                AvailableTo = r.AvailableTo,
                HotelId = r.HotelId,
                Hotel = new HotelDto
                {
                    Id = r.Hotel.Id,
                    Name = r.Hotel.Name,
                    Location = r.Hotel.Location,
                    Description = r.Hotel.Description
                }
            }).ToList();
        }


        public async Task<PagedResult<RoomDto>> GetPaginatedRoomsAsync(int pageNumber, int pageSize)
        {
            var rooms = await _repo.GetAllAsync(pageNumber, pageSize);
            var totalCount = await _repo.CountAsync();

            var roomDtos = rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                Capacity = r.Capacity,
                PricePerNight = r.PricePerNight,
                AvailableFrom = r.AvailableFrom,
                AvailableTo = r.AvailableTo,
                HotelId = r.HotelId,
                Hotel = new HotelDto
                {
                    Id = r.Hotel.Id,
                    Name = r.Hotel.Name,
                    Location = r.Hotel.Location,
                    Description = r.Hotel.Description
                }
            }).ToList();

            return new PagedResult<RoomDto>
            {
                Items = roomDtos,
                TotalCount = totalCount,
                Page = pageNumber,
                PageSize = pageSize
            };
        }


        public async Task<PagedResult<RoomDto>> GetPaginatedRoomsByHotelIdAsync(int hotelId, int page, int pageSize)
        {
            var rooms = await _repo.GetByHotelIdAsync(hotelId, page, pageSize);
            var totalCount = await _repo.CountByHotelIdAsync(hotelId);

            var roomDtos = rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                Capacity = r.Capacity,
                PricePerNight = r.PricePerNight,
                AvailableFrom = r.AvailableFrom,
                AvailableTo = r.AvailableTo,
                HotelId = r.HotelId,
                Hotel = new HotelDto
                {
                    Id = r.Hotel.Id,
                    Name = r.Hotel.Name,
                    Location = r.Hotel.Location,
                    Description = r.Hotel.Description
                }
            }).ToList();

            return new PagedResult<RoomDto>
            {
                Items = roomDtos,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }




        public async Task<RoomDto?> GetRoomByIdAsync(int id)
        {
            // 1. Cache kontrolü
            var cachedRoom = await _cache.GetRoomAsync(id);
            if (cachedRoom != null)
            {
                _logger.LogInformation("✅ Room {RoomId} retrieved from Redis cache.", id);
                return cachedRoom;
            }
           
           
            // 2. Veritabanından al
            var room = await _repo.GetByIdAsync(id);
            if (room == null)
            {
                _logger.LogWarning("⚠️ Room {RoomId} not found in database.", id);
                return null;
            }

            _logger.LogInformation("⏳ Room {RoomId} not found in cache. Fetched from database.", id);

            // 4. DTO olarak döndür
            var roomDto = new RoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Capacity = room.Capacity,
                PricePerNight = room.PricePerNight,
                AvailableFrom = room.AvailableFrom,
                AvailableTo = room.AvailableTo,
                HotelId = room.HotelId,
                Hotel = new HotelDto
                {
                    Id = room.Hotel.Id,
                    Name = room.Hotel.Name,
                    Location = room.Hotel.Location,
                    Description = room.Hotel.Description
                }
            };

            await _cache.SetRoomAsync(roomDto);
            _logger.LogInformation("📦 Room {RoomId} cached to Redis.", id);
            return roomDto;
        }


        public async Task<RoomDto> CreateRoomAsync(RoomDto dto)
        {
            var room = new Room
            {
                RoomNumber = dto.RoomNumber,
                Capacity = dto.Capacity,
                PricePerNight = dto.PricePerNight,
                AvailableFrom = dto.AvailableFrom,
                AvailableTo = dto.AvailableTo,
                HotelId = dto.HotelId
            };

            var createdRoom = await _repo.AddAsync(room);


            // 🏨 Hotel'i al
            var hotel = await _repo.GetHotelByIdAsync(dto.HotelId);

            return new RoomDto
            {
                Id = createdRoom.Id,
                RoomNumber = createdRoom.RoomNumber,
                Capacity = createdRoom.Capacity,
                PricePerNight = createdRoom.PricePerNight,
                AvailableFrom = createdRoom.AvailableFrom,
                AvailableTo = createdRoom.AvailableTo,
                HotelId = createdRoom.HotelId,
                Hotel = hotel == null ? null! : new HotelDto
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    Location = hotel.Location,
                    Description = hotel.Description
                },
            };
        }




        public async Task<Room?> UpdateRoomAsync(int id, RoomDto dto)
        {
            var room = new Room
            {
                RoomNumber = dto.RoomNumber,
                Capacity = dto.Capacity,
                PricePerNight = dto.PricePerNight,
                AvailableFrom = dto.AvailableFrom,
                AvailableTo = dto.AvailableTo,
                HotelId = dto.HotelId
            };

            return await _repo.UpdateAsync(id, room);
        }




        public async Task<bool> DeleteRoomAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }




        // EKLENEN METOT
        public async Task<PagedResult<RoomDto>> SearchAvailableRoomsAsync(RoomSearchDto dto, bool isLoggedIn, int page, int pageSize)
        {
            var rooms = await _repo.GetAllWithHotelAsync();

            var filtered = rooms
                .Where(r =>
                    r.Hotel.Location.ToLower().Contains(dto.Location.ToLower()) &&
                    r.Capacity >= dto.PeopleCount &&
                    r.AvailableFrom <= dto.StartDate &&
                    r.AvailableTo >= dto.EndDate)
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Capacity = r.Capacity,
                    AvailableFrom = r.AvailableFrom,
                    AvailableTo = r.AvailableTo,
                    HotelId = r.HotelId,
                    PricePerNight = isLoggedIn ? r.PricePerNight * 0.85m : r.PricePerNight,
                    Hotel = new HotelDto
                    {
                        Id = r.Hotel.Id,
                        Name = r.Hotel.Name,
                        Location = r.Hotel.Location,
                        Description = r.Hotel.Description
                    }
                })
                .ToList();

            var pagedItems = filtered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            return new PagedResult<RoomDto>
            {
                Items = pagedItems,
                TotalCount = filtered.Count,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<List<RoomReservationSummaryDto>> GetRoomReservationSummariesAsync()
        {
            return await _repo.GetRoomReservationSummariesAsync();
        }
    }
}
