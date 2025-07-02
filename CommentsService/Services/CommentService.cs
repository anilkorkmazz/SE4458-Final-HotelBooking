using CommentsService.DTOs;
using CommentsService.Repositories;

namespace CommentsService.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;

        public CommentService(ICommentRepository repository)
        {
            _repository = repository;
        }

        public Task<List<CommentDto>> GetCommentsByHotelIdAsync(int hotelId)
        {
            return _repository.GetCommentsByHotelIdAsync(hotelId);
        }

        public Task AddCommentAsync(CreateCommentDto dto)
        {
            return _repository.AddCommentAsync(dto);
        }

        public Task<Dictionary<int, int>> GetRatingDistributionAsync(int hotelId)
        {
            return _repository.GetRatingDistributionAsync(hotelId);
        }
    }
}
