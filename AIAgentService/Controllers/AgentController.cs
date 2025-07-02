using AIAgentService.DTOs;
using AIAgentService.Services;
using Microsoft.AspNetCore.Mvc;
using HotelService.Models; // Eğer orada tanımlıysa


namespace AIAgentService.Controllers
{
    [ApiController]
    [Route("api/v1/agent")]
    public class AgentController : ControllerBase
    {
        private readonly IAgentCoordinatorService _coordinatorService;

        public AgentController(IAgentCoordinatorService coordinatorService)
        {
            _coordinatorService = coordinatorService;
        }

        [HttpPost("message")]
        public async Task<IActionResult> HandleUserMessage([FromBody] string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                return BadRequest("Message cannot be empty.");
            }

            try
            {
                var response = await _coordinatorService.HandleMessageAsync(userMessage);
                return Ok(new { message = response });
                
            }

            catch (Exception ex)
            {
                // Detaylı hata mesajı sadece development ortamında gösterilmeli
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
