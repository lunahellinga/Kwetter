using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Events;
using MentionService.Data;
using MentionService.Models;
using MentionService.Services.Interfaces;
using Serilog;

namespace MentionService.Services;

public class MentionManagerService : IMentionManagerService
{
    private readonly DataContext _dataContext;
    private readonly IMentionFinderService _mentionFinderService;

    public MentionManagerService(IMentionFinderService mentionFinderService, DataContext dataContext)
    {
        _mentionFinderService = mentionFinderService;
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

    public async Task<MentionCompleted> Create(MentionCommand mentionCommand)
    {
        Log.Logger.Information("Entered create");
        var mentions = _mentionFinderService.FindMentions(mentionCommand.UserId, mentionCommand.KweetText);
        Log.Logger.Information("Found mentions");
        var mentionCount = mentions.Count;
        Log.Logger.Information("Found {MentionCount} mentions", mentionCount);
        if (mentionCount > 0)
        {
            foreach (var mention in mentions)
            {
                //TODO Actually check if users exist and fetch their ID
                var userId = Guid.NewGuid();

                await _dataContext.Mentions.AddAsync(new Mention
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    KweetId = mentionCommand.CorrelationId
                });
            }

            await _dataContext.SaveChangesAsync();
        }

        // TODO: Convert mentionedusers to a list of IDs, or maybe include both username and id
        return new MentionCompleted
        {
            CorrelationId = mentionCommand.CorrelationId,
            MentionCount = mentionCount,
            MentionedUsers = mentions
        };
    }

    public Task<string> Update(string id, string text)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(RollbackMentions rollback)
    {
        var kweet = await _dataContext.Mentions.FindAsync(rollback.CorrelationId);
        if (kweet == null) throw new ArgumentException("Kweet not found.");
        _dataContext.Mentions.Remove(kweet);
        await _dataContext.SaveChangesAsync();
    }
}