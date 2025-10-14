using ErrorOr;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult MatchAndMapResult<TSource, TDestination>(
    ErrorOr<TSource> result,
    IMapper mapper
)
    {
        return result.Match(
            data => Ok(mapper.Map<TDestination>(data)),
            errors => Problem(detail: string.Join(", ", errors.Select(e => e.Description)))
        );
    }
}
