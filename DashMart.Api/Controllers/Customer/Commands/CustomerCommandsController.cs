using DashMart.Api.Controllers.Abstraction;
using DashMart.Api.Controllers.Customer.Requests;
using DashMart.Application.Customers.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Customer.Commands
{
    [Route("customers/")]
    [ApiController]
    [Authorize]
    public class CustomerCommandsController(IMediator _mediator) : BaseController
    {

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPatch("{Id}/streets/{streetId}")]
        public async Task<IActionResult> AddCustomerAddress
            (Guid Id, Guid streetId, [FromBody] AddCustomerAddressRequest request, CancellationToken cancellationToken)
        {
            var command = new AddCustomerAddressCommand(Id, request.AddressTitle, streetId, request.AddressType, request.HouseNumber, request.BuildingNo);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }



        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCustomer(Guid Id, CancellationToken cancellationToken)
        {
            var command = new DeleteCustomerCommand(Id);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }



        [HttpPost("{Id}/addresses/{addressId}")]
        public async Task<IActionResult> PlaceCustomerOrder(Guid Id, Guid addressId, [FromBody] PlaceCustomerOrderRequest request , CancellationToken cancellationToken)
        {
            var command = new PlaceCustomerOrderCommand(Id, addressId, request.Note);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateCustomerFullName(Guid Id, [FromBody] UpdateCustomerFullNameRequest request , CancellationToken cancellationToken)
        {
            var command = new UpdateCustomerFullNameCommand(Id, request.FirstName, request.LastName);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }



    }
}
