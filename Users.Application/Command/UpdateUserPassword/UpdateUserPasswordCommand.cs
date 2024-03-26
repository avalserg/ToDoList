using MediatR;
using Users.Application.Dtos;

namespace Users.Application.Command.UpdateUserPassword
{
    public class UpdateUserPasswordCommand : IRequest<UpdateUserPasswordDto>
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; } = default!;
    }
}
