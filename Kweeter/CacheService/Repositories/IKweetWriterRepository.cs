using System;
using System.Threading.Tasks;
using CacheService.Models;
using Domain.Events;

namespace CacheService.Repositories;

public interface IKweetWriterRepository
{
    Task<CacheCompleted> Add(CacheKweet cacheKweet);
    Task Delete(Guid id);
    Task Anonymize(Guid userId);
}