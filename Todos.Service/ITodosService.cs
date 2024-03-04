using Common.Domain;
using Todos.Service.Dto;

namespace Todos.Service;

public interface ITodosService
{
    IReadOnlyCollection<Domain.Todos> GetAllToDo(int? offset, string? labelFreeText, int? limit = 10);
    Domain.Todos? GetToDoById(int id);
    Domain.Todos CreateToDo(CreateTodoDto toDo);
    Domain.Todos? UpdateToDo(UpdateToDoDto newToDo);
    int Count(string? labelFreeText);
    bool RemoveToDo(int id);
   
}