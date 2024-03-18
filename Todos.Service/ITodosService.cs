using Todos.Service.Dto;

namespace Todos.Service;

public interface ITodosService
{
    Task<IReadOnlyCollection<Common.Domain.Todos>> GetAllToDoAsync(int? offset, string? labelFreeText, int? limit, bool? descending, CancellationToken cancellationToken = default);
    Task<Common.Domain.Todos?> GetToDoByIdAsync(int id,CancellationToken cancellationToken);
    Task<Common.Domain.Todos?> CreateToDoAsync(CreateTodoDto createToDo, CancellationToken cancellationToken = default);
    Task<Common.Domain.Todos?> UpdateToDoAsync(UpdateToDoDto updateToDo, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string? labelFreeText, CancellationToken cancellationToken = default);
    Task<bool> RemoveToDoAsync(int id, CancellationToken cancellationToken = default);
    
}