using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CacheService.Models;

namespace CacheService.Services.Interfaces;

public interface IKweetService
{
    Task<CacheKweet> GetKweet(Guid id);
    Task<IList<CacheKweet>> GetRecentKweets();
    Task<IList<CacheKweet>> GetKweetsByUsername(string username);
    Task<IList<CacheKweet>> GetKweetsByTag(string tag);
}