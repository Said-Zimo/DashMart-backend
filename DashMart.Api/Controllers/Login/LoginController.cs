using DashMart.Api.Controllers.Abstraction;
using DashMart.Application.Login.Courier;
using DashMart.Application.Login.Customer;
using DashMart.Application.Login.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Login
{
    [Route("login/")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController(IMediator _mediator) : BaseController
    {

        [HttpPost("customers")]
        public async Task<IActionResult> CustomerLoginByPhoneNumber([FromBody] CustomerLoginByPhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPost("users")]
        public async Task<IActionResult> UserLoginByPhoneNumber([FromBody] UserLoginByPhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }


        [HttpPost("couriers")]
        public async Task<IActionResult> CourierLoginByPhoneNumber([FromBody] CourierLoginByPhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return ToActionResult(result);
        }



    }
}
