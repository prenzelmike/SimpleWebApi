namespace SimpleWebApi.Models
{
    public interface ICookbookMongoDbSettings
    {
        string ReceipesCollectionName { get; set; }

        string UnitLookupsCollectionName { get; set; }

        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class CookbookMongoDbSettings : ICookbookMongoDbSettings
    {
        public string ReceipesCollectionName { get; set; }
        public string UnitLookupsCollectionName { get; set; }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}