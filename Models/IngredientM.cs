
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleWebApi.Models
{
    public class IngredientM
    {
        [BsonElement("quantity")]
        public decimal Quantity { get; set; }

        [BsonElement("unit")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Unit { get; set; }

        [JsonProperty("ingredientName")]
        [BsonElement("name")]
        public string Name { get; set; }
    }
}