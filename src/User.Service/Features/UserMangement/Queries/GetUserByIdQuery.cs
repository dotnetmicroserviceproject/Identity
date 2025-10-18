using MediatR;
using User.Service.Features.UserMangement.Dtos;

namespace User.Service.Features.UserMangement.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public Guid UserId { get; }
        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
