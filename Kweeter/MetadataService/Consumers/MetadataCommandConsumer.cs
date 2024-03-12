using System;
using System.Threading.Tasks;
using Domain.Events;
using MassTransit;
using MetadataService.Services.Interfaces;
using Serilog;

namespace MetadataService.Consumers;

public class MetadataCommandConsumer :
    IConsumer<MetadataCommand>
{
    private readonly IMetadataManagerService _metadataManagerService;

    public MetadataCommandConsumer(IMetadataManagerService metadataManagerService)
    {
        _metadataManagerService = metadataManagerService;
    }

    public async Task Consume(ConsumeContext<MetadataCommand> context)
    {
        try
        {
            var message = await _metadataManagerService.Create(context.Message);

            Log.Logger.Information("Metadata stored {@ContextMessage}", message);
            await context.Publish(message);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Exception while storing metadata: {@ContextMessage}", context.Message);
            throw;
        }
    }
}