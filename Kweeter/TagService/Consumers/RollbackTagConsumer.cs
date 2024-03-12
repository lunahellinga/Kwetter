using System;
using System.Threading.Tasks;
using Domain.Events;
using MassTransit;
using Serilog;
using TagService.Services.Interfaces;

namespace TagService.Consumers;

public class RollbackTagConsumer :
    IConsumer<RollbackTags>
{
    private readonly ITagManagerService _tagManagerService;

    public RollbackTagConsumer(ITagManagerService tagManagerService)
    {
        _tagManagerService = tagManagerService;
    }

    public Task Consume(ConsumeContext<RollbackTags> context)
    {
        try
        {
            _tagManagerService.Delete(context.Message);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Log.Logger.Information("Deleting {@Message} caused exception {@Exception}", context.Message, e.Message);
            throw;
        }
    }
}