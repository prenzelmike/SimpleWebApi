using SimpleWebApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;


namespace SimpleWebApi.Services
{
    public class ReceipeService
    {
        private readonly IMongoCollection<ReceipeM> _receipes;
        private readonly IMongoCollection<UnitLookup> _unitLookups;

        public ReceipeService(ICookbookMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            this._receipes = database.GetCollection<ReceipeM>(settings.ReceipesCollectionName);
            this._unitLookups = database.GetCollection<UnitLookup>(settings.UnitLookupsCollectionName);
        }

        public List<UnitLookup> GetUnitLookups()
        {
            return this._unitLookups.Find(lookup=> true).ToList();
        }

        public List<ReceipeMSearchResult> Search(string term)
        {
            return this._receipes
                .Find(Builders<ReceipeM>.Filter.Regex("code",new BsonRegularExpression(term,"i")))
                .Project<ReceipeMSearchResult>("{code: 1}")
                .ToList<ReceipeMSearchResult>();
        }

        public List<ReceipeM> Get()
        {
            //  var result = this._receipes.Aggregate()
            //  .Lookup(foreignCollection: Uni)
            return this._receipes.Find( new BsonDocument())
            .Project<ReceipeM>("{code: 1}")
            .ToList();
        }
        public ReceipeM Get(string id)
        {
            return this._receipes.Find<ReceipeM>(receipe => receipe.Id == id).FirstOrDefault();
        }



        public ReceipeM Create(ReceipeM newReceipe)
        {
            _receipes.InsertOne(newReceipe);
            return newReceipe;
        }

        public void Update(string id, ReceipeM existingReceipe)
        {
            this._receipes.ReplaceOne(receipe => receipe.Id == id, existingReceipe);
        }

        public void Remove(ReceipeM existingReceipe)
        {
            this._receipes.DeleteOne(receipe => receipe.Id == existingReceipe.Id);
        }
        public void Remove(string id)
        {
            this._receipes.DeleteOne(receipe => receipe.Id == id);
        }

    }
}