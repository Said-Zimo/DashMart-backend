using DashMart.Api.Controllers.Abstraction;
using DashMart.Application.Orders.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Order.Queries
{
    [Route("orders/")]
    [ApiController]
    [Authorize]
    public class OrderQueriesController(IMediator _mediator) : BaseController
    {


        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken cancellationToken)
        {
            var command = new GetAllOrdersQuery(pageSize, pageNumber);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOrderById(Guid Id,  CancellationToken cancellationToken)
        {

            var command = new GetOrderByIdQuery(Id);
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


    }
}
