using ErrorOr;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult MatchAndMapOkResult<TSource, TDestination>(
        ErrorOr<TSource>? result,
        IMapper mapper
    )
    {
        if (result is null)
            return NotFound();

        return result?.Match(
            data => Ok(mapper.Map<TDestination>(data)),
            errors => Problem(errors)
        );
    }

    public IActionResult MatchAndMapNoContentResult<TSource>(
        ErrorOr<TSource> result,
        IMapper mapper
    )
    {
        return result.Match<IActionResult>(
            data => NoContent(),
            Problem
        );
    }

    protected IActionResult MatchAndMapCreatedResult<TSource>(
        ErrorOr<TSource> result,
        IMapper mapper
    )
    {
        return result.Match<IActionResult>(
            data => Created("", data),
            errors => Problem(errors)
        );
    }

    protected ObjectResult Problem(List<Error> errors)
    {
        if (errors == null || errors.Count == 0)
        {
            // Defensive fallback
            return new ObjectResult("An unknown error occurred")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = firstError.Description,
            Detail = "See errors property for details.",
            Instance = HttpContext?.Request?.Path.Value ?? ""
        };


        // Add all error descriptions as a list under "errors" property
        problemDetails.Extensions["errors"] = errors.Select(e => new
        {
            code = e.Code, // If your Error has a code
            description = e.Description
        });

        // Optionally add traceId for tracking
        problemDetails.Extensions["traceId"] = HttpContext?.TraceIdentifier ?? "";

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode,
            ContentTypes = { "application/problem+json" }
        };
    }
}
