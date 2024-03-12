using System.Threading.Tasks;
using Domain.Events;

namespace CacheService.Services.Interfaces;

public interface ICacheManager
{
    public Task<CacheCompleted> Create(CacheCommand command);
}