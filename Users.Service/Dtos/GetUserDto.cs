using Common.Domain;

namespace Users.Service.Dto
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Login { get; set; } = default!;
        public IEnumerable<ApplicationUserApplicationRole> Roles { get; set; } = default!;
    }
}
