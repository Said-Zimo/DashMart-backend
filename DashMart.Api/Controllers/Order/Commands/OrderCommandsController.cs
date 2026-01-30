using DashMart.Api.Controllers.Abstraction;
using DashMart.Api.Controllers.Order.Request;
using DashMart.Application.Orders.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Order.Commands
{
    [Route("orders/")]
    [ApiController]
    [Authorize]
    public class OrderCommandsController(IMediator _mediator) : BaseController
    {

        [HttpPost("{Id}/products/{productId}")]
        public async Task<IActionResult> AddItemToOrder(Guid Id, Guid productId, [FromBody] AddItemToOrderRequest request, CancellationToken cancellationToken)
        {
            var command = new AddItemToOrderCommand(Id, productId, request.Quantity);

            var result = await _mediator.Send(command , cancellationToken);

            return ToActionResult(result);
        }


        [HttpPatch("{Id}/status")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ChangeOrderStatus(Guid Id, [FromBody] ChangeOrderStatusRequest request, CancellationToken cancellationToken)
        {
            var command = new ChangeOrderStatusCommand(Id, request.NewOrderStatus);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }



        [HttpDelete("{Id}/products/{productId}")]
        public async Task<IActionResult> DeleteOrderItem(Guid Id, Guid productId, CancellationToken cancellationToken)
        {
            var command = new DeleteOrderItemCommand(Id, productId);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPatch("{Id}/products/{productId}/stock/insert")]
        public async Task<IActionResult> InsertItemQuantity(Guid Id, Guid productId, [FromBody] UpdateItemQuantityRequest request,  CancellationToken cancellationToken)
        {
            var command = new InsertItemQuantityCommand(productId, Id, request.Quantity);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPatch("{Id}/products/{productId}/stock/reduce")]
        public async Task<IActionResult> ReducingItemQuantity(Guid Id, Guid productId, [FromBody] UpdateItemQuantityRequest request, CancellationToken cancellationToken)
        {
            var command = new ReducingItemQuantityCommand(Id, productId, request.Quantity);
            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPatch("{Id}")]
        public async Task<IActionResult> SetOrderNote(Guid Id, [FromBody] SetOrderNoteRequest request, CancellationToken cancellationToken)
        {
            var command = new SetOrderNoteCommand(Id, request.Note);

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

    }
}
