using System;
using System.Threading.Tasks;
using DataService.Services.Interfaces;
using Domain.Events;
using MassTransit;
using Serilog;

namespace DataService.Consumers;

public class RollbackTextConsumer :
    IConsumer<RollbackText>
{
    private readonly ITextService _textService;

    public RollbackTextConsumer(ITextService textService)
    {
        _textService = textService;
    }

    public Task Consume(ConsumeContext<RollbackText> context)
    {
        try
        {
            _textService.Delete(context.Message);
            return Task.CompletedTask;
        }
        catch (ArgumentException e)
        {
            Log.Logger.Information("Message not found for deletion {@Message}", context.Message, e.Message);
            throw;
        }
        catch (Exception e)
        {
            Log.Logger.Information("Deleting {@Message} caused exception {@Exception}", context.Message, e.Message);
            throw;
        }
    }
}