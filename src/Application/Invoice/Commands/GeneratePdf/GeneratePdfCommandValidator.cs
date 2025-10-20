using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Invoice.Commands.GeneratePdf;
public class GeneratePdfCommandValidator : AbstractValidator<GeneratePdfCommand>
{
    public GeneratePdfCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .WithMessage("Id is required.");
    }
}
