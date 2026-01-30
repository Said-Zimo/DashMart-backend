using DashMart.Application.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DashMart.Api.Controllers.Abstraction
{
    [Route("DashMart/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);

            return result.StatusCode switch
            {
                StatusCodeEnum.BadRequest =>
                   BadRequest(result.ErrorMessages),

                StatusCodeEnum.NotFound =>
                    NotFound(result.ErrorMessages),

                StatusCodeEnum.Conflict =>
                    Conflict(result.ErrorMessages),

                StatusCodeEnum.Forbidden =>
                    StatusCode(StatusCodes.Status403Forbidden, result.ErrorMessages),

                StatusCodeEnum.Unauthorized => 
                Unauthorized(result.ErrorMessages),

                _ => throw new InvalidOperationException
                     ($"Unhandled status code: {result.StatusCode}")
            };
        }
    }
}
