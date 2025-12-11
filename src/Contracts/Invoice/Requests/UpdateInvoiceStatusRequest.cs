using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Invoices;

namespace Contracts.Invoice.Requests;
public record UpdateInvoiceStatusRequest(InvoiceStatus IsPaid);
