using HotelService.DTOs;
using HotelService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HotelService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService service)
        {
            _service = service;
        }

        /*
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _service.GetPaginatedRoomsAsync(page, pageSize));
        }

        */


        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? hotelId = null 
        )
        {
            if (hotelId.HasValue)
            {
                var result = await _service.GetPaginatedRoomsByHotelIdAsync(hotelId.Value, page, pageSize);
                return Ok(result);
            }

            return Ok(await _service.GetPaginatedRoomsAsync(page, pageSize));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _service.GetRoomByIdAsync(id);
            return room == null ? NotFound() : Ok(room);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] RoomDto dto)
        {
            var created = await _service.CreateRoomAsync(dto);
            return Ok(created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDto dto)
        {
            var updated = await _service.UpdateRoomAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var deleted = await _service.DeleteRoomAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAvailableRooms(
            [FromQuery] string location,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int peopleCount,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            
            if (string.IsNullOrWhiteSpace(location))
                return BadRequest("Konum (location) boş olamaz.");

            if (peopleCount < 1)
                return BadRequest("Kişi sayisi en az 1 olmalidir.");

            if (startDate > endDate)
                return BadRequest("Başlangiç tarihi, bitiş tarihinden sonra olamaz.");

            if (page < 1 || pageSize < 1)
                return BadRequest("Sayfa numarasi ve sayfa boyutu pozitif olmalidir.");

            var isLoggedIn = User.Identity?.IsAuthenticated ?? false;

            var dto = new RoomSearchDto
            {
                Location = location,
                StartDate = startDate,
                EndDate = endDate,
                PeopleCount = peopleCount
            };

            var result = await _service.SearchAvailableRoomsAsync(dto, isLoggedIn, page, pageSize);

            // 🔍 Hiç uygun oda bulunmadıysa bilgi döndür (200 OK ile ama anlamlı mesajla)
            if (result.Items.Count == 0)
                return Ok(new { message = "Uygun oda bulunamadi.", data = result });

            return Ok(result);
        }
        
        [HttpGet("reservation-summary")]
        public async Task<IActionResult> GetRoomReservationSummaries()
        {
            var result = await _service.GetRoomReservationSummariesAsync();
            return Ok(result);
        }


        
    }
}
