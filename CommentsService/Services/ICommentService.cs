using CommentsService.DTOs;

namespace CommentsService.Services
{
    public interface ICommentService
    {
        Task<List<CommentDto>> GetCommentsByHotelIdAsync(int hotelId);
        Task AddCommentAsync(CreateCommentDto dto);
        Task<Dictionary<int, int>> GetRatingDistributionAsync(int hotelId);
    }
}
