using MediatR;
using User.Service.Features.UserMangement.Dtos;

namespace User.Service.Features.UserMangement.Commands
{
    public class UserSignUpCommand : IRequest<SignUpResponse>
    {
        public UserSignUpDto SignUpBody { get; }
        public UserSignUpCommand(UserSignUpDto signUpBody)
        {
            SignUpBody = signUpBody;
        }
    }
}
