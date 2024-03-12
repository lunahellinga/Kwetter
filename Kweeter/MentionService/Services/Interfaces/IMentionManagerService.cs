using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Events;

namespace MentionService.Services.Interfaces;

public interface IMentionManagerService
{
    public Task<List<string>> GetAll();
    public Task<string> Get(string id);
    public Task<MentionCompleted> Create(MentionCommand mentionCommand);
    public Task<string> Update(string id, string text);
    public Task Delete(RollbackMentions rollback);
}