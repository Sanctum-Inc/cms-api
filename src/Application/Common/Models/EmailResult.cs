using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Emails;
using Domain.Users;

namespace Application.Common.Models;
public class EmailResult
{
    // Recipients
    public required string ToAddresses { get; set; }
    public string? CcAddresses { get; set; }
    public string? BccAddresses { get; set; }

    // Content
    public required string Subject { get; set; } = default!;
    public required string Body { get; set; } = default!;

    // Attachments / metadata
    public string? AttachmentIds { get; set; }

    // Status
    public required EmailStatus Status { get; set; }
    public string? ErrorMessage { get; set; }

    // Audit
    public DateTime? SentAt { get; set; }
}
