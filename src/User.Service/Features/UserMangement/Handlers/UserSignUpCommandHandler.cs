using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using User.Contracts;
using User.Service.Entities;
using User.Service.Features.UserMangement.Commands;
using User.Service.Features.UserMangement.Dtos;



namespace User.Service.Features.UserMangement.Handlers
{
    public class UserSignUpCommandHandler : IRequestHandler<UserSignUpCommand, SignUpResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint publishEndpoint;
        public UserSignUpCommandHandler(IMapper mapper, IPublishEndpoint publishEndpoint, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _mapper = mapper;
            this.publishEndpoint = publishEndpoint;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<SignUpResponse> Handle(UserSignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // ✅ Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(request.SignUpBody.Email);
                if (existingUser is not null)
                {
                    return new SignUpResponse
                    {
                        Message = "Email already exists",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Data = null

                    };
                }

                // ✅ Map and initialize user
                var createdUser = _mapper.Map<ApplicationUser>(request.SignUpBody);
                createdUser.UserName = request.SignUpBody.Email;
                createdUser.Email = request.SignUpBody.Email;
                createdUser.Id = Guid.NewGuid();

                // ✅ Let Identity handle password hashing internally
                var result = await _userManager.CreateAsync(createdUser, request.SignUpBody.Password);

                if (!result.Succeeded)
                {
                    return new SignUpResponse
                    {
                        Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                // ✅ Assign roles (auto-assign Admin by email)
                var adminEmails = new List<string>
                {
                    "brightelo1@gmail.com",
                    "admin2@example.com",
                    "manager@example.com"
                };

                string assignedRole = adminEmails.Contains(request.SignUpBody.Email.ToLower())
                    ? Roles.Admin
                    : Roles.Customer;


                // ✅ Ensure role exists (using ApplicationRole)
                var existingRole = await _roleManager.FindByNameAsync(assignedRole);
                if (existingRole == null)
                {
                    var newRole = new ApplicationRole
                    {
                        Id = Guid.NewGuid(),
                        Name = assignedRole,
                        NormalizedName = assignedRole.ToUpper()
                    };
                    await _roleManager.CreateAsync(newRole);
                }

                await _userManager.AddToRoleAsync(createdUser, assignedRole);

                await publishEndpoint.Publish(new ApplicationUserCreated(
                    createdUser.Id,
                    createdUser.Email    
                ));

                SignupResponseData signUpResponseData = GetUserDetails(createdUser);

                return new SignUpResponse
                {
                    Message = "User registered successfully",
                    Data = signUpResponseData,
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (Exception)
            {
                return new SignUpResponse
                {
                    Message = "An error occurred while processing your request",
                };
            }
        }

        private SignupResponseData GetUserDetails(ApplicationUser createdUser)
        {
            var user = _mapper.Map<UserResponseDto>(createdUser);
            var signUpResponseData = new SignupResponseData { User = user };
            return signUpResponseData;
        }
    }

}
