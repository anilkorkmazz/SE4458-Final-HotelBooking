using HotelService.DTOs;
using HotelService.Models;
using HotelService.Repositories;

namespace HotelService.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _repo;

        public HotelService(IHotelRepository repo)
        {
            _repo = repo;
        }


        /*
        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            return await _repo.GetAllAsync();
        }
        */


        public async Task<List<HotelDto>> GetAllHotelsAsync(string? location = null)
        {
            var hotels = await _repo.GetAllAsync(location);
            
            return hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Location = h.Location,
                Description = h.Description
            }).ToList();
        }


        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Hotel> CreateHotelAsync(HotelDto dto)
        {
            var hotel = new Hotel
            {
                Name = dto.Name,
                Location = dto.Location,
                Description = dto.Description
            };
            return await _repo.AddAsync(hotel);
        }

        public async Task<Hotel?> UpdateHotelAsync(int id, HotelDto dto)
        {
            var hotel = new Hotel
            {
                Name = dto.Name,
                Location = dto.Location,
                Description = dto.Description
            };
            return await _repo.UpdateAsync(id, hotel);
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        
        public async Task<PagedResult<Hotel>> GetPagedHotelsAsync(int page, int pageSize)
        {
            var items = await _repo.GetPagedAsync(page, pageSize);
            var total = await _repo.GetTotalCountAsync();

            return new PagedResult<Hotel>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
