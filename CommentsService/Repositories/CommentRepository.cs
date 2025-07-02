using CommentsService.DTOs;
using CommentsService.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Microsoft.Extensions.Options;


namespace CommentsService.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IMongoCollection<Comment> _collection;

        public CommentRepository(IOptions<MongoDBSettings> mongoSettings)
        {
            var settings = mongoSettings.Value;
            
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Comment>(settings.CommentsCollectionName);
        }

        public async Task<List<CommentDto>> GetCommentsByHotelIdAsync(int hotelId)
        {
            var comments = await _collection.Find(c => c.HotelId == hotelId).ToListAsync();

            return comments.Select(c => new CommentDto
            {
                UserName = c.UserName,
                Text = c.Text,
                Rating = c.Rating,
                CreatedAt = c.CreatedAt
            }).ToList();
        }

        public async Task AddCommentAsync(CreateCommentDto dto)
        {
            var comment = new Comment
            {
                HotelId = dto.HotelId,
                UserName = dto.UserName,
                Text = dto.Text,
                Rating = dto.Rating,
                CreatedAt = DateTime.UtcNow
            };

            await _collection.InsertOneAsync(comment);
        }

        public async Task<Dictionary<int, int>> GetRatingDistributionAsync(int hotelId)
        {
            var comments = await _collection.Find(c => c.HotelId == hotelId).ToListAsync();
            return comments
                .GroupBy(c => c.Rating)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
