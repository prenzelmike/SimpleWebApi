using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleWebApi.Models
{
    public class ReceipeM
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("ingredients")]
        public IngredientM[] Ingredients { get; set; }

        [BsonElement("workflow")]
        public string Workflow { get; set; }

    }
}