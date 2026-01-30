using Azure.Core;
using DashMart.Api.Controllers.Abstraction;
using DashMart.Application.Couriers.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Courier.Queries
{
    [Route("couriers/")]
    [ApiController]
    [Authorize]
    public class CourierQueriesController(IMediator _mediator) : BaseController
    {

        [HttpGet("streets/{Id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllCouriersByStreetId
            (Guid Id,[FromQuery] int pageSize,[FromQuery] int pageNumber , CancellationToken cancellationToken)
        {
            var query = new GetAllCouriersByStreetIdQuery(Id, pageSize, pageNumber);

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCourierById(Guid Id, CancellationToken cancellationToken)
        {
            var query = new GetCourierByIdQuery(Id);

            var result = await _mediator.Send(query, cancellationToken);

            return ToActionResult(result);
        }




    }
}
