using Azure.Core;
using DashMart.Api.Controllers.Abstraction;
using DashMart.Api.Controllers.Courier.Requests;
using DashMart.Application.Couriers.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Courier.Commands
{
    [Route("couriers/")]
    [ApiController]
    [Authorize]
    public class CourierCommandsController(IMediator _mediator) : BaseController
    {

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCourier([FromBody] CreateCourierCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPut("{Id}/driver-license")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeactivateDriverLicense(Guid Id, [FromBody] DeactivateDriverLicenseRequest request, CancellationToken cancellationToken)
        {
            var command = new DeactivateDriverLicenseCommand(Id, request.DriverLicenseNumber);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


        [HttpDelete("{Id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteCourier(Guid Id, CancellationToken cancellationToken)
        {
            var command = new DeleteCourierCommand(Id);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPatch("{Id}/order/{orderId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SetCourierToOrder(Guid Id, Guid orderId, CancellationToken cancellationToken)
        {
            var command = new SetCourierToOrderCommand(Id, orderId);
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateCourierFullName(Guid Id, [FromBody] UpdateCourierFullNameRequest request,  CancellationToken cancellationToken)
        {
            var command = new UpdateCourierFullNameCommand(Id, request.FirstName, request.LastName);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }




    }
}
