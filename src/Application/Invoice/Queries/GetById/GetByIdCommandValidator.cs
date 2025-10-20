using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Invoice.Queries.GetById;
public class GetByIdCommandValidator : AbstractValidator<GetByIdCommand>
{
    public GetByIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .WithMessage("Id is required.");
    }
}
