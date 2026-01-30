using DashMart.Api.Controllers.Abstraction;
using DashMart.Application.Categories.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Category.Queries
{
    [Route("categories/")]
    [ApiController]
    [AllowAnonymous]
    public class CategoryQueriesController(IMediator _mediator) : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> GetAllCategory( [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken cancellationToken)
        {
            var query = new GetAllCategoryQuery(pageSize, pageNumber);

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCategoryById(Guid Id, CancellationToken cancellationToken)
        {
            var query = new GetCategoryByIdQuery(Id);

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }


    }
}
