using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Events;
using Microsoft.EntityFrameworkCore;
using TagService.Data;
using TagService.Models;
using TagService.Services.Interfaces;

namespace TagService.Services;

public class TagManagerService : ITagManagerService
{
    private readonly DataContext _dataContext;
    private readonly ITagFinderService _tagFinderService;

    public TagManagerService(ITagFinderService tagFinderService, DataContext dataContext)
    {
        _tagFinderService = tagFinderService;
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

    public async Task<TagCompleted> Create(TagCommand kweet)
    {
        var tags = _tagFinderService.FindTags(kweet.UserId, kweet.KweetText);

        // TODO: Cache the existing tags
        foreach (var tag in tags)
        {
            var foundTag = await _dataContext.Tags.Where(t => t.Text == tag.Text).FirstOrDefaultAsync();
            if (foundTag == null)
            {
                foundTag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Text = tag.Text
                };
                await _dataContext.Tags.AddAsync(foundTag);
            }

            var tagUse = new TagUse
            {
                KweetId = kweet.CorrelationId,
                TagId = foundTag.Id
            };
            await _dataContext.TagUses.AddAsync(tagUse);
        }

        await _dataContext.SaveChangesAsync();

        return new TagCompleted
        {
            CorrelationId = kweet.CorrelationId,
            Tags = tags.Select(t => t.Text).ToList(),
            TagCount = tags.Count
        };
    }

    public Task<string> Update(string id, string text)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(RollbackTags rollback)
    {
        var tagUses = await _dataContext.TagUses.Where(t => t.KweetId == rollback.CorrelationId).ToListAsync();

        _dataContext.TagUses.RemoveRange(tagUses);
        await _dataContext.SaveChangesAsync();
    }
}