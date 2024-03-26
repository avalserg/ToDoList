using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Command.CreateUser;
using Users.Application.Command.RemoveUser;
using Users.Application.Command.UpdateUser;
using Users.Application.Command.UpdateUserPassword;
using Users.Application.Query.GetCountUsers;
using Users.Application.Query.GetListUsers;
using Users.Application.Query.GetUserById;

namespace Users.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
          
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync(
            [FromQuery] GetListUsersQuery getListUsersQuery,
            CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(getListUsersQuery, cancellationToken);
            var countUsers = await _mediator.Send(
                new GetCountUsersQuery() { NameFreeText = getListUsersQuery.NameFreeText },
                cancellationToken);
            HttpContext.Response.Headers.Append("X-Total-Count", countUsers.ToString());
            return Ok(users);
        }
        [AllowAnonymous]
        [HttpGet("totalCount")]
        public async Task<IActionResult> GetCountUserAsync(string? labelFreeText, CancellationToken cancellationToken)
        {

            var users = await _mediator.Send(new GetCountUsersQuery() { NameFreeText = labelFreeText },
                cancellationToken);

            return Ok(users);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id,CancellationToken cancellationToken)
        {

            var user = await _mediator.Send(new GetUserByIdQuery(){Id=id},cancellationToken);

            return Ok(user);
        }
    
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync(
            CreateUserCommand createUserCommand, 
            CancellationToken cancellationToken)
        {
            var user = await  _mediator.Send(createUserCommand, cancellationToken);

            return Created($"users/{user.Id}", user);
        }
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UpdateUserCommand updateUserCommand, CancellationToken cancellationToken)
        {
            
            updateUserCommand.Id = id;
            
            var updatedUser = await _mediator.Send(new UpdateUserCommand(){Id = id,Login = updateUserCommand.Login}, cancellationToken);

            return Ok(updatedUser);
        }

        [HttpPut("{id}/Password")]
        public async Task<IActionResult> UpdateUserPasswordAsync(int id, UpdateUserPasswordCommand updateUserPasswordCommand, CancellationToken cancellationToken)
        {
            updateUserPasswordCommand.Id = id;
           

            var updatedUser = await _mediator.Send(updateUserPasswordCommand, cancellationToken);

            return Ok(updatedUser);

        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserAsync([FromBody] int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemoveUserCommand() { Id = id}, cancellationToken);

            return Ok($"User with ID = {id} was deleted");
        }
    }

}
