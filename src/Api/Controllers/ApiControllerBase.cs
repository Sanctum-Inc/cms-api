using ErrorOr;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult MatchAndMapOkResult<TSource, TDestination>(
        ErrorOr<TSource> result,
        IMapper mapper
    )
    {
        return result.Match(
            data => Ok(mapper.Map<TDestination>(data!)),
            errors => Problem(errors)
        );
    }

    protected IActionResult MatchAndMapNoContentResult<TSource, TDestination>(
        ErrorOr<TSource> result,
        IMapper mapper
    )
    {
        return result.Match<IActionResult>(
            data => NoContent(),
            errors => Problem(errors)
            );
    }
    protected IActionResult MatchAndMapCreatedResult<TSource, TDestination>(
        ErrorOr<TSource> result,
        IMapper mapper
    )
    {
        return result.Match<IActionResult>(
            data => Created(),
            errors => Problem(errors)
            );
    }

    private ObjectResult Problem(List<Error> errors)
    {
        // Take the first error to determine the status code
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

        return Problem(
            statusCode: statusCode,
            detail: string.Join(", ", errors.Select(e => e.Description))
        );
    }
}
