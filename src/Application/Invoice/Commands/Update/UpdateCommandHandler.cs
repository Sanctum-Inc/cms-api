using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.Update;
public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
