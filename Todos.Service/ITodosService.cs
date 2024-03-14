using Todos.Service.Dto;

namespace Todos.Service;

public interface ITodosService
{
    IReadOnlyCollection<Common.Domain.Todos> GetAllToDo(int? offset, string? labelFreeText, int? limit = 10);
    Task<IReadOnlyCollection<Common.Domain.Todos>> GetAllToDoAsync(int? offset, string? labelFreeText, int? limit, bool? descending, CancellationToken cancellationToken = default);
    Common.Domain.Todos? GetToDoById(int id);
    Task<Common.Domain.Todos?> GetToDoByIdAsync(int id,CancellationToken cancellationToken);
    Common.Domain.Todos CreateToDo(CreateTodoDto toDo);
    Task<Common.Domain.Todos?> CreateToDoAsync(CreateTodoDto createToDo, CancellationToken cancellationToken = default);
    Common.Domain.Todos? UpdateToDo(UpdateToDoDto newToDo);
    Task<Common.Domain.Todos?> UpdateToDoAsync(UpdateToDoDto updateToDo, CancellationToken cancellationToken = default);
    int Count(string? labelFreeText);
    Task<int> CountAsync(string? labelFreeText, CancellationToken cancellationToken = default);
    bool RemoveToDo(int id);
    Task<bool> RemoveToDoAsync(int id, CancellationToken cancellationToken = default);


}