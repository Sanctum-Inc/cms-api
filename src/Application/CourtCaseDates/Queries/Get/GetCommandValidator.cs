using FluentValidation;

namespace Application.CourtCaseDates.Queries.Get;

public class GetCommandValidator : AbstractValidator<GetCommand>
{
    public GetCommandValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Id is required.");
    }
}
