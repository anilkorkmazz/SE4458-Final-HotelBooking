using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using CommentsService.Models;

namespace CommentsService.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        }

        public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("Comments");
    }
}
