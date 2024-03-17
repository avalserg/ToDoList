namespace Common.Domain
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Login { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public IEnumerable<ApplicationUserApplicationRole> Roles { get; set; }= default!;
        public List<Todos> Todos { get; set; } = default!;
    }
}
