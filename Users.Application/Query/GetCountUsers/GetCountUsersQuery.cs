using MediatR;
using Users.Application.Dtos;

namespace Users.Application.Query.GetCountUsers
{
    public class GetCountUsersQuery:BaseUsersFilter,IRequest<int>
    {
    }
}
