using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackGammonModels
{
    public class User
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        [BsonId]
        public BsonObjectId? _id { get; set; }
    }
}