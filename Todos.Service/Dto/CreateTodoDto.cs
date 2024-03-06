namespace Todos.Service.Dto
{
    public class CreateTodoDto
    {
        public string Label { get; set; } = default!;
        public int OwnerId { get; set; }

    }
}
