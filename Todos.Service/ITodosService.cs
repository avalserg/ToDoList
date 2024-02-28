namespace Todos.Service;

public interface ITodosService
{
    IReadOnlyCollection<Domain.Todos> GetAllToDo(int? offset, int? ownerId, string? labelFreeText, int? limit = 10);
    Domain.Todos? GetToDoById(int id);
    Domain.Todos AddToDo(Domain.Todos toDo, int ownerId);
    Domain.Todos? UpdateToDo(int id, Domain.Todos newToDo);
    Domain.Todos UpdateToDoIsDone(int id, bool isDone);
    Domain.Todos RemoveToDo(int id);
}