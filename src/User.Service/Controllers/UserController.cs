using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.Service.Features.UserMangement.Dtos;
using User.Service.Features.UserMangement.Queries;

namespace User.Service.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Get User by user Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var response = await _mediator.Send(query);
            return response is null
                ? NotFound(new
                {
                    message = "User not found",
                    is_successful = false,
                    status_code = 404
                })
                : Ok(response);
        }

        /// <summary>
        /// Get all User
        /// </summary>
        /// <returns></returns>

        [HttpGet("")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }
    }
}
