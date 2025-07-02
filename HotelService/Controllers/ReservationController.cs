using HotelService.DTOs;
using HotelService.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationController(IReservationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Book([FromBody] ReservationRequestDto dto)
        {
            try
            {
                var result = await _service.BookReservationAsync(dto);
                return Ok(result);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            
        }
    }
}
