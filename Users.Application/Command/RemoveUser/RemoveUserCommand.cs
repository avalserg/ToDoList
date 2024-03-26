using MediatR;
using Users.Application.Dtos;

namespace Users.Application.Command.RemoveUser
{
    public class RemoveUserCommand:IRequest<bool>
    {
        public int Id { get; set; }
    }
}
