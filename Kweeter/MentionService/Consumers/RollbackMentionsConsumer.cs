using System;
using System.Threading.Tasks;
using Domain.Events;
using MassTransit;
using MentionService.Services.Interfaces;
using Serilog;

namespace MentionService.Consumers;

public class RollbackMentionsConsumer :
    IConsumer<RollbackMentions>
{
    private readonly IMentionManagerService _mentionManagerService;

    public RollbackMentionsConsumer(IMentionManagerService mentionManagerService)
    {
        _mentionManagerService = mentionManagerService;
    }

    public Task Consume(ConsumeContext<RollbackMentions> context)
    {
        Log.Logger.Information("Entering mention process");
        try
        {
            _mentionManagerService.Delete(context.Message);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Log.Logger.Information("Deleting {@Message} caused exception {@Exception}", context.Message, e.Message);
            throw;
        }
    }
}