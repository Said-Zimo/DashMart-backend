using DashMart.Application.Exceptions;
using DashMart.Application.Results;
using DashMart.Domain.Abstraction;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;


namespace DashMart.Api.Exception
{
    public sealed class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, System.Exception exception, CancellationToken cancellationToken)
        {
            context.Response.ContentType = "application/json";

            Result<string> response;

            switch (exception)
            {
                case AppValidationException ex:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = Result<string>.Failure(ex.Errors.Select(x => $"{x.Key} : {string.Join(" | ", x.Value)}").ToList(), StatusCodeEnum.BadRequest);
                    break;

                case ValidationException ex:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = Result<string>.Failure(ex.Errors.Select(x => $"{x.PropertyName} : {x.ErrorMessage}").ToList(), StatusCodeEnum.BadRequest);
                    break;

                case DomainException ex:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = Result<string>.Failure(ex.Message, StatusCodeEnum.BadRequest);
                    break;

                default:
                    context.Response.StatusCode = 500;
                    response = Result<string>.Failure("Unexpected error", StatusCodeEnum.InternalServerError);
                    break;

            }

            await context.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }

    }
}
