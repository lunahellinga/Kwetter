using System;
using System.Threading.Tasks;
using ContentService.Services.Interfaces;
using Domain.Events;
using MassTransit;
using Serilog;

namespace ContentService.Consumers;

public class RollbackContentConsumer :
    IConsumer<RollbackContent>
{
    private readonly IContentManagerService _contentManagerService;

    public RollbackContentConsumer(IContentManagerService contentManagerService)
    {
        _contentManagerService = contentManagerService;
    }

    public Task Consume(ConsumeContext<RollbackContent> context)
    {
        try
        {
            _contentManagerService.Delete(context.Message);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Log.Logger.Information("Deleting links for {@Message} caused exception {@Exception}", context.Message,
                e.Message);
            throw;
        }
    }
}