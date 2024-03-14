namespace Common.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public UserRole UserRole { get; set; }= default!;
        public int UserRoleId { get; set; }
        public List<Todos> Todos { get; set; } = default!;
    }
}
