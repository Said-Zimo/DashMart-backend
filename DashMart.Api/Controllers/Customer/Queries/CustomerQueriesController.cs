using DashMart.Api.Controllers.Abstraction;
using DashMart.Application.Customers.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Customer.Queries
{
    [Route("customers/")]
    [ApiController]
    [Authorize]
    public class CustomerQueriesController(IMediator _mediator) : BaseController
    {

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCustomer(Guid Id , CancellationToken cancellationToken)
        {
            var query = new GetCustomerQuery(Id);

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }


        [HttpGet("orders/{Id}")]
        public async Task<IActionResult> GetAllCustomerOrdersByCustomerId
            (Guid Id, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken cancellationToken)
        {
            var query = new GetAllCustomerOrdersByCustomerIdQuery(Id, pageNumber, pageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return ToActionResult(result);
        }


        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllCustomers([FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken cancellationToken)
        {
            var query = new GetAllCustomersQuery(pageSize, pageNumber);
            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);

        }

    }
}
