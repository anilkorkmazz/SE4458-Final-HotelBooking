using HotelService.Data;
using HotelService.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelDbContext _context;

        public HotelRepository(HotelDbContext context)
        {
            _context = context;
        }


        /*
        public async Task<List<Hotel>> GetAllAsync()
        {
            return await _context.Hotels.ToListAsync();
        }

        */

        public async Task<List<Hotel>> GetAllAsync(string? location)
        {
            var query = _context.Hotels.AsQueryable();

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(h => h.Location.ToLower().Contains(location.ToLower()));
            }

            return await query.ToListAsync();
        }





        public async Task<Hotel?> GetByIdAsync(int id)
        {
            return await _context.Hotels.FindAsync(id);
        }

        public async Task<Hotel> AddAsync(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }

        public async Task<Hotel?> UpdateAsync(int id, Hotel hotel)
        {
            var existing = await _context.Hotels.FindAsync(id);
            if (existing == null) return null;

            existing.Name = hotel.Name;
            existing.Location = hotel.Location;
            existing.Description = hotel.Description;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null) return false;

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<List<Hotel>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Hotels
                .OrderBy(h => h.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Hotels.CountAsync();
        }
    }
}
