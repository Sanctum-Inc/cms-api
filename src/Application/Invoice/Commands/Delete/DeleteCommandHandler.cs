using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.Delete;
public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
