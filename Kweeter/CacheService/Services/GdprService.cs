using System;
using System.Threading.Tasks;
using CacheService.Repositories;
using CacheService.Services.Interfaces;

namespace CacheService.Services;

public class GdprService : IGdprService
{
    private readonly IKweetWriterRepository _kweetWriterRepository;

    public GdprService(IKweetWriterRepository kweetWriterRepository)
    {
        _kweetWriterRepository = kweetWriterRepository;
    }

    public async Task Anonymize(Guid userId)
    {
        await _kweetWriterRepository.Anonymize(userId);
    }
}