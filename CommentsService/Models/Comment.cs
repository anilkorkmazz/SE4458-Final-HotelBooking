using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommentsService.Models
{
    public class Comment
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("hotelId")]
        public int HotelId { get; set; }

        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("rating")]
        public int Rating { get; set; } // 1-5 arası puan

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
