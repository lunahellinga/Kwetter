using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CacheService.Models;
using CacheService.Repositories;
using CacheService.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CacheService.Services;

public class KweetService : IKweetService
{
    private readonly IDistributedCache _cache;
    private readonly string _keyFmt = ":catalog:{0}:{1}";
    private readonly IKweetReaderRepository _kweetReaderRepository;

    public KweetService(IKweetReaderRepository kweetReaderRepository, IDistributedCache cache)
    {
        _kweetReaderRepository = kweetReaderRepository;
        _cache = cache;
    }

    public async Task<CacheKweet> GetKweet(Guid id)
    {
        return await GetFromCacheWithHardExpire<CacheKweet>(
            "kweets",
            id.ToString(),
            60,
            async () => await _kweetReaderRepository.GetKweet(id));
    }

    public async Task<IList<CacheKweet>> GetRecentKweets()
    {
        return await GetFromCacheWithHardExpire<IList<CacheKweet>>(
            "kweets",
            "recent",
            15,
            async () => await _kweetReaderRepository.GetRecentKweets());
    }

    public async Task<IList<CacheKweet>> GetKweetsByUsername(string username)
    {
        return await GetFromCacheWithHardExpire<IList<CacheKweet>>(
            "kweets",
            username,
            30,
            async () => await _kweetReaderRepository.GetKweetsByUsername(username));
    }

    public async Task<IList<CacheKweet>> GetKweetsByTag(string tag)
    {
        return await GetFromCacheWithHardExpire<IList<CacheKweet>>(
            "kweets",
            tag,
            30,
            async () => await _kweetReaderRepository.GetKweetsByTag(tag));
    }

    private async Task<TResult> GetFromCacheWithHardExpire<TResult>(
        string key,
        string val,
        int expiration,
        Func<Task<object>> func)
    {
        var cacheKey = string.Format(_keyFmt, key, val);
        var data = await _cache.GetStringAsync(cacheKey);

        if (string.IsNullOrEmpty(data))
        {
            data = JsonConvert.SerializeObject(await func());

            var encodedData = Encoding.UTF8.GetBytes(data);
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(expiration));
            await _cache.SetAsync(
                cacheKey,
                encodedData,
                options);
        }

        return JsonConvert.DeserializeObject<TResult>(data);
    }
}