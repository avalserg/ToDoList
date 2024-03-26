namespace Users.Application.Dtos
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Login { get; set; } = default!;
        
    }
}
