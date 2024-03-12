using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.Data;

public class MongoInitializer
{
    private IOptions<MongoDbSettings> _dbSettings;

    public MongoInitializer(IOptions<MongoDbSettings> dbSettings)
    {
        _dbSettings = dbSettings;
    }

    public void Init()
    {
        var mongoClient = new MongoClient(
            _dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            _dbSettings.Value.DatabaseName);

        var filter = new BsonDocument("name", _dbSettings.Value.CollectionName);
        var options = new ListCollectionNamesOptions { Filter = filter };

        if (!mongoDatabase.ListCollectionNames(options).Any())
        {
            mongoDatabase.CreateCollection(_dbSettings.Value.CollectionName);
        }
    }
}