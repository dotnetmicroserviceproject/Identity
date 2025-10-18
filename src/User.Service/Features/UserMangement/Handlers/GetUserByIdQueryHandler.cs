using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using User.Service.Entities;
using User.Service.Features.UserMangement.Dtos;
using User.Service.Features.UserMangement.Queries;

namespace User.Service.Features.UserMangement.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            this._userManager = userManager;
        }
        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            // ✅ Find user by ID (Identity expects string)
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());


            if (user == null)
            {
                return null;
            }
        
            return _mapper.Map<UserDto>(user);
        }
    }
}
