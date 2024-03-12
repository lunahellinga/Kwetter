using System;
using System.Threading.Tasks;
using Domain.Events;
using MassTransit;
using MentionService.Services.Interfaces;
using Serilog;

namespace MentionService.Consumers;

public class MentionCommandConsumer :
    IConsumer<MentionCommand>
{
    private readonly IMentionManagerService _mentionManagerService;

    public MentionCommandConsumer(IMentionManagerService mentionManagerService)
    {
        _mentionManagerService = mentionManagerService;
    }

    public async Task Consume(ConsumeContext<MentionCommand> context)
    {
        Log.Logger.Information("Starting mention storage");
        try
        {
            var message = await _mentionManagerService.Create(context.Message);

            Log.Logger.Information("Mentions stored {@ContextMessage}", message);
            // await Task.Delay(4000);
            await context.Publish(message);
            // return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Exception while storing message: {@ContextMessage}", context.Message);
            throw;
        }
    }
}