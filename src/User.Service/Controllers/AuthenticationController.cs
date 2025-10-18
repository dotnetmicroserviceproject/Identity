using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.Service.Features.UserMangement.Commands;
using User.Service.Features.UserMangement.Dtos;

namespace User.Service.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates User
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SignUpResponse>> UserSignUp([FromBody] UserSignUpDto body)
        {
            var command = new UserSignUpCommand(body);
            var response = await _mediator.Send(command);

            if (response.Data == null)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(UserSignUp), response);
        }

        /// <summary>
        /// Logs in User
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>

        [HttpPost("login")]
        [ProducesResponseType(typeof(UserLoginResponseDto<SignupResponseData>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserLoginResponseDto<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginResponseDto<SignupResponseData>>> Login([FromBody] UserLoginRequestDto loginRequest)
        {
            var command = new CreateUserLoginCommand(loginRequest);
            var response = await _mediator.Send(command);
            if (response == null || response.Data == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid credentials",
                    error = "Invalid email or password",
                    status_code = StatusCodes.Status401Unauthorized

                });

            }
            return Ok(response);
        }
    }
}
