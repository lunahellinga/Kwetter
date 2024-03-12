using Microsoft.Extensions.Options;
using Mongo.Data;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.Services;

public class KweetService
{
    private readonly IMongoCollection<Kweet> _kweetsCollection;

    public KweetService(
        IOptions<MongoDbSettings> kweetDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            kweetDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            kweetDatabaseSettings.Value.DatabaseName);

        _kweetsCollection = mongoDatabase.GetCollection<Kweet>(
            kweetDatabaseSettings.Value.CollectionName);
    }

    public async Task<List<Kweet>> GetAsync()
    {
        return await _kweetsCollection.Find(_ => true)
            .Sort(Builders<Kweet>.Sort.Descending(x => x.Timestamp))
            .Limit(100)
            .ToListAsync();
    }

    public async Task<Kweet?> GetAsync(Guid id)
    {
        return await _kweetsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Kweet kweet)
    {
        await _kweetsCollection.InsertOneAsync(kweet);
    }

    public async Task UpdateAsync(Kweet kweet)
    {
        await _kweetsCollection.ReplaceOneAsync(x => x.Id == kweet.Id, kweet);
    }

    public async Task RemoveAsync(Guid id)
    {
        await _kweetsCollection.DeleteOneAsync(x => x.Id == id);
    }
}