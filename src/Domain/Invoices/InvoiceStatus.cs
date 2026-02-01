using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Invoices;
public enum InvoiceStatus
{
    PENDING,
    SENT,
    PAID,
    OVERDUE,
    CANCELLED,
    PARTIALLY_PAID,
    DRAFT,
}
