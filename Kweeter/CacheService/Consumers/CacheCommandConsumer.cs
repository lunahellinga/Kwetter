using System;
using System.Threading.Tasks;
using CacheService.Services.Interfaces;
using Domain.Events;
using MassTransit;
using Serilog;

namespace CacheService.Consumers;

public class CacheCommandConsumer :
    IConsumer<CacheCommand>
{
    private readonly ICacheManager _cacheManager;

    public CacheCommandConsumer(ICacheManager cacheManager)
    {
        _cacheManager = cacheManager;
    }

    public async Task Consume(ConsumeContext<CacheCommand> context)
    {
        try
        {
            var message = await _cacheManager.Create(context.Message);

            Log.Logger.Information("Cached {@ContextMessage}", context.Message);
            await context.Publish(message);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Exception while caching: {@ContextMessage}", context.Message);
            throw;
        }
    }
}