using System;
using System.Threading.Tasks;
using DataService.Services.Interfaces;
using Domain.Events;
using MassTransit;
using Serilog;

namespace DataService.Consumers;

public class TextCommandConsumer :
    IConsumer<TextCommand>
{
    private readonly ITextService _textService;

    public TextCommandConsumer(ITextService textService)
    {
        _textService = textService;
    }

    public async Task Consume(ConsumeContext<TextCommand> context)
    {
        try
        {
            var message = await _textService.Create(context.Message);


            Log.Logger.Information("Message stored {@ContextMessage}", context.Message);

            await context.Publish(message);
            // return Task.CompletedTask;
        }
        catch (ArgumentException e)
        {
            Log.Logger.Information(e, "Message failed to pass validation: {@ContextMessage}", context.Message);
            throw;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Exception while storing message: {@ContextMessage}", context.Message);
            throw;
        }
    }
}