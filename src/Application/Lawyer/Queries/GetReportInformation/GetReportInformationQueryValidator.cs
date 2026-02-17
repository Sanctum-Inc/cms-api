using FluentValidation;

namespace Application.Lawyer.Queries.GetReportInformation;

public class GetReportInformationQueryValidator : AbstractValidator<GetReportInformationQuery>
{
    public GetReportInformationQueryValidator()
    {
        RuleFor(x => x.LawyerId)
            .NotEmpty()
            .WithMessage("LawyerId is required")
            .NotNull()
            .WithMessage("LawyerId is required");
    }
}
