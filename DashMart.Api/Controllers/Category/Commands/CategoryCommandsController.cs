using DashMart.Api.Controllers.Abstraction;
using DashMart.Api.Controllers.Category.Requests;
using DashMart.Application.Categories.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Category.Commands
{
    [Route("categories/")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class CategoryCommandsController(IMediator _mediator) : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateCategoryName(Guid Id, [FromBody] UpdateCategoryNameRequest request,  CancellationToken cancellationToken)
        {
            var command = new UpdateCategoryNameCommand(Id, request.Name);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


    }
}
