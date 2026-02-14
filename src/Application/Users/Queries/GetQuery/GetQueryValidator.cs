using FluentValidation;

namespace Application.Users.Queries;

public class GetQueryValidator : AbstractValidator<GetQuery>
{
    public GetQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("User ID is required.")
            .NotEqual(Guid.Empty.ToString()).WithMessage("User ID cannot be an empty GUID.");
    }
}
