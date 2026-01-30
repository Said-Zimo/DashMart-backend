using DashMart.Api.Controllers.Abstraction;
using DashMart.Application.Users.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.User.Queries
{
    [Route("users/")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserQueriesController(IMediator _mediator) : BaseController
    {


        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken cancellationToken)
        {
            var command = new GetAllUsersQuery(pageSize, pageNumber);
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserById(Guid Id, CancellationToken cancellationToken)
        {
            var command = new GetUserByIdQuery(Id);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


    }
}
