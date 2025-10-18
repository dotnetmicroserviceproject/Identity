using MediatR;
using User.Service.Features.UserMangement.Dtos;

namespace User.Service.Features.UserMangement.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {

    }
}
