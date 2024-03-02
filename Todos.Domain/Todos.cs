namespace Todos.Domain
{
    public class Todos
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsDone { get; set; }
        public string Label { get; set; } = default!;
        public int OwnerId { get; set; }

    }

}
