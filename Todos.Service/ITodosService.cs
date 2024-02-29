namespace Todos.Service;

public interface ITodosService
{
    IReadOnlyCollection<Domain.Todos> GetAllToDo(int? offset, int? ownerId, string? labelFreeText, int? limit = 10);
    Domain.Todos? GetToDoById(int id);
    Domain.Todos AddToDo(Domain.Todos toDo);
    Domain.Todos? UpdateToDo(int id, Domain.Todos newToDo);
    bool RemoveToDo(int id);
}