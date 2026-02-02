using FluentValidation;

namespace Application.CourtCaseDates.Queries.GetById;

public class GetByIdCommandValidator : AbstractValidator<GetByIdCommand>
{
    public GetByIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
