using FluentValidation;
using MediatR;
using ErrorOr;

namespace Application.Common.Behaviours;
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            // Check if TResponse is ErrorOr<T>
            var responseType = typeof(TResponse);
            if (responseType.IsGenericType &&
                responseType.GetGenericTypeDefinition() == typeof(ErrorOr<>))
            {
                // Convert validation failures to ErrorOr errors
                var errors = failures
                    .Select(failure => Error.Validation(
                        code: failure.PropertyName,
                        description: failure.ErrorMessage))
                    .ToList();

                // Use reflection to call the implicit conversion from List<Error> to ErrorOr<T>
                var errorOrType = typeof(ErrorOr<>).MakeGenericType(responseType.GetGenericArguments()[0]);
                var implicitOperator = errorOrType.GetMethod(
                    "op_Implicit",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                    null,
                    new[] { typeof(List<Error>) },
                    null);

                return (TResponse)implicitOperator!.Invoke(null, new object[] { errors })!;
            }

            // Fallback to exception if not using ErrorOr
            throw new ValidationException(failures);
        }

        return await next(cancellationToken);
    }
}
