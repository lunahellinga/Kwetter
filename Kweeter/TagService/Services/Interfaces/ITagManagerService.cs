using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Events;

namespace TagService.Services.Interfaces;

public interface ITagManagerService
{
    public Task<List<string>> GetAll();
    public Task<string> Get(string id);
    public Task<TagCompleted> Create(TagCommand kweet);
    public Task<string> Update(string id, string text);
    public Task Delete(RollbackTags rollback);
}