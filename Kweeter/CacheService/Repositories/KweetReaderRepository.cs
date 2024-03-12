using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CacheService.Data;
using CacheService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CacheService.Repositories;

public class KweetReaderRepository : IKweetReaderRepository
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<CacheKweet> _kweetsCollection;

    public KweetReaderRepository(IOptions<MongoOptions> cfg)
    {
        _client = new MongoClient(cfg.Value.ConnectionString);
        _db = _client.GetDatabase(cfg.Value.DatabaseName);
        _kweetsCollection = _db.GetCollection<CacheKweet>(
            cfg.Value.CollectionName);
    }

    public async Task<CacheKweet> GetKweet(Guid id)
    {
        return await _kweetsCollection.Find(x => x.KweetId == id.ToString()).FirstOrDefaultAsync();
    }

    public async Task<IList<CacheKweet>> GetRecentKweets()
    {
        return await _kweetsCollection.Find(_ => true)
            .Sort(Builders<CacheKweet>.Sort.Descending(x => x.CreatedAt))
            .Limit(100).ToListAsync();
    }

    public async Task<IList<CacheKweet>> GetKweetsByUsername(string username)
    {
        return await _kweetsCollection.Find(x => x.UserName == username)
            .Sort(Builders<CacheKweet>.Sort.Descending(x => x.CreatedAt))
            .Limit(100).ToListAsync();
    }

    public async Task<IList<CacheKweet>> GetKweetsByTag(string tag)
    {
        return await _kweetsCollection.Find(x => x.Tags.Contains(tag))
            .Sort(Builders<CacheKweet>.Sort.Descending(x => x.CreatedAt))
            .Limit(100).ToListAsync();
    }
}