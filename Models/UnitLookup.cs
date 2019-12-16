using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleWebApi.Models {
    public class UnitLookup{

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {get; set;}
        public string DisplayValue {get; set;}
    }
}