using System.ComponentModel.DataAnnotations;

namespace CommentsService.DTOs
{
    public class CreateCommentDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "HotelId must be a positive number")]
        public int HotelId { get; set; }
    }
}
