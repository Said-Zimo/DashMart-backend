using DashMart.Api.Controllers.Abstraction;
using DashMart.Api.Controllers.User.Requests;
using DashMart.Application.Users.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.User.Commands
{
    [Route("users/")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserCommandsController(IMediator _mediator) : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(Guid Id, CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand(Id);
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUserPermissions
            (Guid Id, [FromBody] UpdateUserPermissionsRequest request,  CancellationToken cancellationToken)
        {
            var command = new UpdateUserPermissionsCommand(Id,request.permissions);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

    }
}
