using DashMart.Api.Controllers.Abstraction;
using DashMart.Application.Carts.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Cart.Commands
{
    [Route("carts/")]
    [ApiController]
    public class CartCommandsController : BaseController
    {

        private readonly IMediator _mediator;

        public CartCommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        
        [HttpPost("items")]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

        

        [HttpDelete("items/{CartId}/{ProductId}")]
        public async Task<IActionResult> DeleteCartItem(
            Guid CartId, 
            Guid ProductId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteCartItemCommand(CartId, ProductId);

            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }

    }
}
