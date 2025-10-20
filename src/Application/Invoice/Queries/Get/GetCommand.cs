using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Queries.Get;
public record GetCommand() : IRequest<ErrorOr<IEnumerable<InvoiceResult>>>;
