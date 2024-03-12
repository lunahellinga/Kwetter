using System;
using System.Threading.Tasks;
using Domain.Events;
using MassTransit;
using MetadataService.Services.Interfaces;
using Serilog;

namespace MetadataService.Consumers;

public class RollbackMetadataConsumer :
    IConsumer<RollbackMetadata>
{
    private readonly IMetadataManagerService _metadataManagerService;

    public RollbackMetadataConsumer(IMetadataManagerService metadataManagerService)
    {
        _metadataManagerService = metadataManagerService;
    }

    public Task Consume(ConsumeContext<RollbackMetadata> context)
    {
        try
        {
            _metadataManagerService.Delete(context.Message);
            Log.Logger.Information("Deleted {@Message}", context.Message);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Log.Logger.Information("Deleting {@Message} caused exception {@Exception}", context.Message, e.Message);
            throw;
        }
    }
}