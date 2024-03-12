using System;
using System.Threading.Tasks;
using CacheService.Data;
using CacheService.Models;
using Domain.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CacheService.Repositories;

public class KweetWriterRepository : IKweetWriterRepository
{
    private readonly IMongoCollection<CacheKweet> _kweetsCollection;

    public KweetWriterRepository(IOptions<MongoOptions> cfg)
    {
        var client = new MongoClient(cfg.Value.ConnectionString);
        var db = client.GetDatabase(cfg.Value.DatabaseName);
        _kweetsCollection = db.GetCollection<CacheKweet>(
            cfg.Value.CollectionName);
    }

    public async Task<CacheCompleted> Add(CacheKweet cacheKweet)
    {
        await _kweetsCollection.InsertOneAsync(cacheKweet);
        return new CacheCompleted
        {
            CorrelationId = new Guid(cacheKweet.KweetId)
        };
    }

    public async Task Delete(Guid id)
    {
        await _kweetsCollection.DeleteOneAsync(x => x.KweetId == id.ToString());
    }

    public async Task Anonymize(Guid userId)
    {
        var filter = Builders<CacheKweet>.Filter
            .Eq(kweet => kweet.UserId, userId.ToString());
        var update = Builders<CacheKweet>.Update
            .Set(kweet => kweet.UserId, Guid.Empty.ToString())
            .Set(kweet => kweet.UserName, "Deleted");
        await _kweetsCollection.UpdateManyAsync(filter, update);
    }
}