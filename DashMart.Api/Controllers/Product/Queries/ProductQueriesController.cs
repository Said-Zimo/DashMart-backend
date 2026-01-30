using DashMart.Api.Controllers.Abstraction;
using DashMart.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Product.Queries
{
    [Route("products/")]
    [ApiController]
    [AllowAnonymous]
    public class ProductQueriesController(IMediator _mediator) : BaseController
    {

        [HttpGet("categories/{Id}")]
        public async Task<IActionResult> GetProducts(
             Guid Id,
             CancellationToken cancellationToken,
             [FromQuery] int pageSize = 10,
             [FromQuery] int pageNumber = 1)
        {

            var request = new GetAllProductsByCategoryIdQuery(Id, pageSize, pageNumber);
            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProduct(Guid Id, CancellationToken cancellationToken)
        {
            var request = new GetProductByIdQuery(Id);

            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }

    }
}
