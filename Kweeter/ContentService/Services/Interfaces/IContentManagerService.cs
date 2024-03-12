using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Events;

namespace ContentService.Services.Interfaces;

public interface IContentManagerService
{
    public Task<List<string>> GetAll();
    public Task<string> Get(string id);
    public Task<ContentCompleted> Create(ContentCommand kweet);
    public Task<string> Update(string id, string text);
    public Task Delete(RollbackContent rollback);
}