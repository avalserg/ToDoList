using Common.Domain;
using Todos.Service.Dto;

namespace Todos.Service;

public interface ITodosService
{
    IReadOnlyCollection<Common.Domain.Todos> GetAllToDo(int? offset, string? labelFreeText, int? limit = 10);
    Task<IReadOnlyCollection<Common.Domain.Todos>> GetAllToDoAsync(int? offset, string? labelFreeText, int? limit);
    Common.Domain.Todos? GetToDoById(int id);
    Task<Common.Domain.Todos?> GetToDoByIdAsync(int id,CancellationToken cancellationToken);
    Common.Domain.Todos CreateToDo(CreateTodoDto toDo);
    Task<Common.Domain.Todos?> CreateToDoAsync(CreateTodoDto createToDo);
    Common.Domain.Todos? UpdateToDo(UpdateToDoDto newToDo);
    int Count(string? labelFreeText);
    bool RemoveToDo(int id);
   
}