using MediatR;
using Users.Application.Dtos;

namespace Users.Application.Query.GetUserById
{
    public class GetUserByIdQuery:IRequest<GetUserDto>
    {
        public int Id { get; set; }
    }
}
