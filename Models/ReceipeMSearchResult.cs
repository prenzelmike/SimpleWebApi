using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleWebApi.Models
{
        public class ReceipeMSearchResult
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("code")]
        public string Abstract { get; set; }
    }
}
