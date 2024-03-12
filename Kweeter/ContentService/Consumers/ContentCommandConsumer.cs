using System;
using System.Threading.Tasks;
using ContentService.Services.Interfaces;
using Domain.Events;
using MassTransit;
using Serilog;

namespace ContentService.Consumers;

public class ContentCommandConsumer :
    IConsumer<ContentCommand>
{
    private readonly IContentManagerService _contentManagerService;

    public ContentCommandConsumer(IContentManagerService contentManagerService)
    {
        _contentManagerService = contentManagerService;
    }

    public async Task Consume(ConsumeContext<ContentCommand> context)
    {
        try
        {
            var message = await _contentManagerService.Create(context.Message);

            Log.Logger.Information("Links stored {@ContextMessage}", message);
            // await Task.Delay(2000);
            await context.Publish(message);
            // return  Task.CompletedTask;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Exception while storing links: {@ContextMessage}", context.Message);
            throw;
        }
    }
}