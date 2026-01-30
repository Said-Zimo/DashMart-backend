using DashMart.Api.Controllers.Abstraction;
using DashMart.Application.Carts.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Cart.Queries
{
    [Route("carts")]
    [ApiController]
    [Authorize]
    public class CartQueriesController(IMediator _mediator) : BaseController
    {

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllCarts([FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken cancellationToken)
        {
            var query = new GetAllCartsQuery(pageSize, pageNumber);

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCartById(Guid Id, CancellationToken cancellationToken)
        {
            var query = new GetCartByIdQuery(Id);

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }


    }
}
