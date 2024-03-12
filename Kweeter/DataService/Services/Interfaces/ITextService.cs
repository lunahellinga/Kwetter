using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Events;

namespace DataService.Services.Interfaces;

public interface ITextService
{
    public Task<List<string>> GetAll();
    public Task<string> Get(string id);
    public Task<TextCompleted> Create(TextCommand kweet);
    public Task<string> Update(string id, string text);
    public Task Delete(RollbackText rollback);
}