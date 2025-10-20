using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.Add;
public class AddCommandHandler : IRequestHandler<AddCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
