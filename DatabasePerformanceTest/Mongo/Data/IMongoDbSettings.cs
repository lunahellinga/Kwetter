namespace Mongo.Data;

public interface IMongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }

    public string CollectionName { get; set; }
}