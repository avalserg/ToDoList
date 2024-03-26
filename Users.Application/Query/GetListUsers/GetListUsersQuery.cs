using MediatR;
using Users.Application.Dtos;

namespace Users.Application.Query.GetListUsers
{
    public class GetListUsersQuery:BaseUsersFilter,IRequest<IReadOnlyCollection<GetUserDto>>
    {
        public int?  Offset { get; set; }
        public int?  Limit { get; set; }
        public bool? Descending { get; set; }
    }
}
