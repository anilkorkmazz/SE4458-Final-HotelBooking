using CommentsService.DTOs;
using CommentsService.Models;

namespace CommentsService.Repositories
{
    public interface ICommentRepository
    {
        Task<List<CommentDto>> GetCommentsByHotelIdAsync(int hotelId);
        Task AddCommentAsync(CreateCommentDto dto);
        Task<Dictionary<int, int>> GetRatingDistributionAsync(int hotelId); // Rating=Key, Count=Value
    }
}
