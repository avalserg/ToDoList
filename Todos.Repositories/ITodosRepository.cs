namespace Todos.Repositories;

public interface ITodosRepository
{
    IReadOnlyCollection<Domain.Todos> GetAllToDo(int? offset, int? ownerId, string? labelFreeText);
    Domain.Todos? GetToDoById(int id);
    Domain.Todos AddToDo(Domain.Todos toDo);
    Domain.Todos? UpdateToDo(int id, Domain.Todos newToDo);
    bool RemoveToDo(int id);
}