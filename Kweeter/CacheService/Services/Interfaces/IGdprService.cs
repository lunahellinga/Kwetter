using System;
using System.Threading.Tasks;

namespace CacheService.Services.Interfaces;

public interface IGdprService
{
    Task Anonymize(Guid userId);
}