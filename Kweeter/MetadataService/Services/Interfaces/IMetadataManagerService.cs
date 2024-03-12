using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Events;

namespace MetadataService.Services.Interfaces;

public interface IMetadataManagerService
{
    public Task<List<string>> GetAll();
    public Task<string> Get(string id);
    public Task<MetadataCompleted> Create(MetadataCommand metadataCommand);
    public Task<string> Update(string id, string text);
    public Task Delete(RollbackMetadata rollback);
}