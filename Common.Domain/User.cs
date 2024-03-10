namespace Common.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public List<Todos> Todos { get; set; }
    }
}
