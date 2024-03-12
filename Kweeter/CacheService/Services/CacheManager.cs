using System.Threading.Tasks;
using CacheService.Models;
using CacheService.Repositories;
using CacheService.Services.Interfaces;
using Domain.Events;

namespace CacheService.Services;

public class CacheManager : ICacheManager
{
    private readonly IKweetWriterRepository _writerRepository;

    public CacheManager(IKweetWriterRepository writerRepository)
    {
        _writerRepository = writerRepository;
    }

    public async Task<CacheCompleted> Create(CacheCommand command)
    {
        await _writerRepository.Add(new CacheKweet
        {
            KweetId = command.CorrelationId.ToString(),
            UserId = command.UserId.ToString(),
            UserName = command.UserName,
            KweetText = command.KweetText,
            Tags = command.Tags,
            MentionedUsers = command.MentionedUsers,
            ContentLinks = command.ContentLinks,
            CreatedAt = command.CreatedAt
        });

        return new CacheCompleted
        {
            CorrelationId = command.CorrelationId
        };
    }
}