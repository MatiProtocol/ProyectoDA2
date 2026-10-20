using System.Diagnostics.CodeAnalysis;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VirtualParkApi.Filters;

[ExcludeFromCodeCoverage]
public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            DbCreatedUniqueOrKeyException e => new ConflictObjectResult(e.Message),

            ResourceNotFoundException e => NotFoundResult(e),

            RewardNotAvailableException e => BadRequestResult(e),

            InsufficientPointsException e => BadRequestResult(e),

            MembershipLevelRequirementException e => BadRequestResult(e),

            ArgumentNullException { InnerException: DbKeyDoesNotExistException } ex => NotFoundResult(ex),

            InvalidOperationException
            { InnerException: ArgumentException { ParamName: "ticket" } ex }
                => NotFoundResult(ex),

            InvalidOperationException
            { InnerException: ArgumentException { ParamName: "Id" } ex }
                => NotFoundResult(ex),

            InvalidOperationException
            { InnerException: ArgumentException { ParamName: "Type" } ex }
                => new BadRequestObjectResult(ex.Message),

            InvalidOperationException
            { InnerException: ArgumentException { ParamName: "Role" } ex }
                => BadRequestResult(ex),

            InvalidOperationException
            { InnerException: ArgumentException { ParamName: "Password" } ex }
                => ForbiddenResult(ex),

            InvalidOperationException ioEx => BadRequestResult(ioEx),

            _ => new ObjectResult("Internal Error") { StatusCode = StatusCodes.Status500InternalServerError }
        };

        context.ExceptionHandled = true;
    }

    private static ObjectResult NotFoundResult(Exception ex)
    {
        return new ObjectResult(new ProblemDetails
        {
            Title = "Not Found",
            Detail = ex.Message,
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5"
        })
        {
            StatusCode = StatusCodes.Status404NotFound
        };
    }

    private static ObjectResult BadRequestResult(Exception ex)
    {
        return new ObjectResult(new ProblemDetails
        {
            Title = "Bad Request",
            Detail = ex.Message,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request"
        })
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }

    private static ObjectResult ForbiddenResult(Exception ex)
    {
        return new ObjectResult(new ProblemDetails
        {
            Title = "Forbidden",
            Detail = ex.Message,
            Status = StatusCodes.Status403Forbidden,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-403-forbidden"
        })
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }
}
