using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataService.Data;
using DataService.Models;
using DataService.Services.Interfaces;
using Domain.Events;

namespace DataService.Services;

public class TextService : ITextService
{
    private readonly DataContext _dataContext;
    private readonly ISanitizerService _sanitizerService;

    public TextService(ISanitizerService sanitizerService, DataContext dataContext)
    {
        _sanitizerService = sanitizerService;
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

    public async Task<TextCompleted> Create(TextCommand kweet)
    {
        if (!_sanitizerService.Validate(kweet.UserId, kweet.KweetText))
            throw new ArgumentException("Validation failed.");

        var text = _sanitizerService.Sanitize(kweet.UserId, kweet.KweetText);

        await _dataContext.Kweets.AddAsync(new KweetSubmission(kweet.CorrelationId, text));
        await _dataContext.SaveChangesAsync();

        return new TextCompleted
        {
            CorrelationId = kweet.CorrelationId,
            KweetText = text
        };
    }

    public Task<string> Update(string id, string text)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(RollbackText rollback)
    {
        var kweet = await _dataContext.Kweets.FindAsync(rollback.CorrelationId);
        if (kweet == null) throw new ArgumentException("Kweet not found.");
        _dataContext.Kweets.Remove(kweet);
        await _dataContext.SaveChangesAsync();
    }
}