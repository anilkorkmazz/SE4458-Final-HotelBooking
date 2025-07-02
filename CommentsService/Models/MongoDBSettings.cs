namespace CommentsService.Models
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CommentsCollectionName { get; set; } = null!;
    }
}
