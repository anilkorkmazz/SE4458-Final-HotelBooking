using HotelService.Data;
using HotelService.Models;
using Microsoft.EntityFrameworkCore;
using HotelService.DTOs;


namespace HotelService.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext _context;

        public RoomRepository(HotelDbContext context)
        {
            _context = context;
        }

        // Tüm odaları getir
        public async Task<List<Room>> GetAllAsync() =>
            await _context.Rooms
                .Include(r => r.Hotel)
                .ToListAsync();




        // Sayfalı şekilde odaları getir
        public async Task<List<Room>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }



        // Belirli bir odayı getir
        public async Task<Room?> GetByIdAsync(int id) =>
            await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == id);



        // Yeni oda ekle
        public async Task<Room> AddAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }




        // Odayı güncelle
        public async Task<Room?> UpdateAsync(int id, Room room)
        {
            var existing = await _context.Rooms.FindAsync(id);
            if (existing == null) return null;

            existing.RoomNumber = room.RoomNumber;
            existing.Capacity = room.Capacity;
            existing.PricePerNight = room.PricePerNight;
            existing.AvailableFrom = room.AvailableFrom;
            existing.AvailableTo = room.AvailableTo;
            existing.HotelId = room.HotelId;

            await _context.SaveChangesAsync();
            return existing;
        }



        // Oda sil
        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }



        // Toplam oda sayısını getir (pagination için)
        public async Task<int> CountAsync()
        {
            return await _context.Rooms.CountAsync();
        }

        public async Task<List<Room>> GetAllWithHotelAsync()
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .ToListAsync();
        }



        public async Task<Hotel?> GetHotelByIdAsync(int hotelId)
        {
            return await _context.Hotels.FindAsync(hotelId);
        }

        public async Task<List<RoomReservationSummaryDto>> GetRoomReservationSummariesAsync()
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.Reservations)
                .Select(r => new RoomReservationSummaryDto
                {
                    RoomId = r.Id,
                    HotelId = r.HotelId,
                    Capacity = r.Capacity,
                    ReservedCount = r.Reservations.Sum(res => res.PeopleCount),
                    RoomNumber = r.RoomNumber,
                    HotelName = r.Hotel.Name

                }).ToListAsync();
        }


        public async Task<List<Room>> GetByHotelIdAsync(int hotelId, int page, int pageSize)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Where(r => r.HotelId == hotelId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountByHotelIdAsync(int hotelId)
        {
            return await _context.Rooms
                .Where(r => r.HotelId == hotelId)
                .CountAsync();
        }
    }
}
