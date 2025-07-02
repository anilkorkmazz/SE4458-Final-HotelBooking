using CommentsService.DTOs;
using CommentsService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommentsService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: /api/v1/comments/hotel/5
        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetCommentsByHotel(int hotelId)
        {
            var comments = await _commentService.GetCommentsByHotelIdAsync(hotelId);
            return Ok(comments);
        }

        // POST: /api/v1/comments
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto dto)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                    
                return BadRequest(new
                {
                    message = "Doğrulama hatalari oluştu.",
                    errors = errors
                });
            }

            await _commentService.AddCommentAsync(dto);
            return Ok(new { message = "Yorum başarıyla eklendi." });
        }

        // GET: /api/v1/comments/distribution/5
        [HttpGet("distribution/{hotelId}")]
        public async Task<IActionResult> GetRatingDistribution(int hotelId)
        {
            var distribution = await _commentService.GetRatingDistributionAsync(hotelId);
            return Ok(distribution);
        }
    }
}
