namespace Todos.Service.Dto
{
    public class UpdateToDoDto
    {
        public int Id { get; set; }
        public bool IsDone { get; set; }
        public string Label { get; set; } = default!;
       
    }
}
