using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentService.Data;
using ContentService.Models;
using ContentService.Services.Interfaces;
using Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace ContentService.Services;

public class ContentManagerService : IContentManagerService
{
    private readonly IContentFinderService _contentFinderService;
    private readonly DataContext _dataContext;

    public ContentManagerService(DataContext dataContext, IContentFinderService contentFinderService)
    {
        _dataContext = dataContext;
        _contentFinderService = contentFinderService;
    }

    public Task<List<string>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<string> Get(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<ContentCompleted> Create(ContentCommand kweet)
    {
        var content = _contentFinderService.GetContent(kweet.UserId, kweet.KweetText);
        var contentCount = content.Count;

        if (contentCount > 0)
        {
            foreach (var link in content) await _dataContext.Content.AddAsync(new Content(kweet.CorrelationId, link));

            await _dataContext.SaveChangesAsync();
        }

        return new ContentCompleted
        {
            CorrelationId = kweet.CorrelationId,
            ContentCount = contentCount,
            ContentLinks = content
        };
    }

    public Task<string> Update(string id, string text)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(RollbackContent rollback)
    {
        var content = await _dataContext.Content.Where(c => c.KweetId == rollback.CorrelationId).ToListAsync();
        _dataContext.Content.RemoveRange(content);
        await _dataContext.SaveChangesAsync();
    }
}