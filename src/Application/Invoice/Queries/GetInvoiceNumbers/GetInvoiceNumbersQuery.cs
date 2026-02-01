using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Queries.GetInvoiceNumbers;
public record GetInvoiceNumbersQuery() : IRequest<ErrorOr<IEnumerable<InvoiceNumbersResult>>>;
