using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Events;
using MetadataService.Data;
using MetadataService.Models;
using MetadataService.Services.Interfaces;

namespace MetadataService.Services;

public class MetadataManagerService : IMetadataManagerService
{
    private readonly DataContext _dataContext;

    public MetadataManagerService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public Task<List<string>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<string> Get(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<MetadataCompleted> Create(MetadataCommand metadataCommand)
    {
        var finalized = DateTime.UtcNow;
        await _dataContext.AddAsync(new Metadata
        {
            KweetId = metadataCommand.CorrelationId,
            UserId = metadataCommand.UserId,
            ContentCount = metadataCommand.ContentCount,
            TagCount = metadataCommand.TagCount,
            MentionCount = metadataCommand.MentionCount,
            CreatedAt = finalized
        });

        await _dataContext.SaveChangesAsync();

        return new MetadataCompleted
        {
            CorrelationId = metadataCommand.CorrelationId,
            CreatedAt = finalized
        };
    }

    public Task<string> Update(string id, string text)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(RollbackMetadata rollback)
    {
        var metadata = await _dataContext.Metadata.FindAsync(rollback.CorrelationId);
        if (metadata == null) throw new Exception("Metadata not found");
        _dataContext.Metadata.Remove(metadata);
        await _dataContext.SaveChangesAsync();
    }
}