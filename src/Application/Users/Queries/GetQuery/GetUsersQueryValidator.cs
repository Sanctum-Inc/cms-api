using FluentValidation;

namespace Application.Users.Queries.GetQuery;

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("User ID is required.")
            .NotEqual(Guid.Empty.ToString()).WithMessage("User ID cannot be an empty GUID.");
    }
}
