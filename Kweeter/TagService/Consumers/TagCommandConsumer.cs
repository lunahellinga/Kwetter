using System;
using System.Threading.Tasks;
using Domain.Events;
using MassTransit;
using Serilog;
using TagService.Services.Interfaces;

namespace TagService.Consumers;

public class TagCommandConsumer :
    IConsumer<TagCommand>
{
    private readonly ITagManagerService _tagManagerService;

    public TagCommandConsumer(ITagManagerService tagManagerService)
    {
        _tagManagerService = tagManagerService;
    }

    public async Task Consume(ConsumeContext<TagCommand> context)
    {
        try
        {
            var message = await _tagManagerService.Create(context.Message);

            Log.Logger.Information("Tags stored {@ContextMessage}", message);
            await context.Publish(message);
            // return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Exception while storing tags: {@ContextMessage}", context.Message);
            throw;
        }
    }
}