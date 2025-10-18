using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.Service.Context;
using User.Service.Entities;
using User.Service.Features.UserMangement.Commands;
using User.Service.Features.UserMangement.Dtos;

namespace User.Service.Features.UserMangement.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<CreateUserLoginCommand, UserLoginResponseDto<SignupResponseData>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpcontextAccessor;
        public LoginUserCommandHandler(IMapper mapper, IHttpContextAccessor httpcontextAccessor, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _mapper = mapper;
            _httpcontextAccessor = httpcontextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<UserLoginResponseDto<SignupResponseData>> Handle(CreateUserLoginCommand request, CancellationToken cancellationToken)
        {
            var normalizedEmail = request.LoginRequestDto.Email.Trim().ToLowerInvariant();

            var user = await _userManager.Users
               .Include(u => u.UserRoles)
               .ThenInclude(ur => ur.Role)
               .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail, cancellationToken);

            if (user == null)
            {
                return new UserLoginResponseDto<SignupResponseData>
                {
                    Data = null,
                    Message = "Invalid credentials",
                    StatusCode = StatusCodes.Status401Unauthorized,
                };
            }

            // ✅ Verify password using Identity
            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, request.LoginRequestDto.Password, lockoutOnFailure: false);

            if (!passwordCheck.Succeeded)
            {
                return new UserLoginResponseDto<SignupResponseData>
                {
                    Message = "Invalid email or password",
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            if (user.Status == "Inative")
            {
                return new UserLoginResponseDto<SignupResponseData>
                {
                    Data = null,
                    Message = "Your account is inactive. Please contact support",
                    StatusCode = StatusCodes.Status403Forbidden,
                };
            }
            if (user.Status == "Deleted")
            {
                return new UserLoginResponseDto<SignupResponseData>
                {
                    Data = null,
                    Message = "Your account does not exist or has been deleted",
                    StatusCode = StatusCodes.Status403Forbidden,
                };
            }

            var lastLogin = new LastLogin
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                LoginTime = DateTime.UtcNow,
                IPAddress = _httpcontextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
            };
            await _context.LastLogins.AddAsync(lastLogin, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new UserLoginResponseDto<SignupResponseData>
            {
                Data = GetUserDetails(user),
                Message = "login successful"

            };
        }

        private SignupResponseData GetUserDetails(ApplicationUser user)
        {
            var userResponse = _mapper.Map<UserResponseDto>(user);

            var signUpResponseData = new SignupResponseData { User = userResponse };
            return signUpResponseData;
        }
    }

}
