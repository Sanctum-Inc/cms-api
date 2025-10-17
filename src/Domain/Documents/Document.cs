using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.CourtCases;
using Domain.Users;

namespace Domain.Documents;

public class Document : AuditableEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public required long Size { get; set; }

    // Foreign Keys
    public required Guid UserId { get; set; }
    public User? User { get; set; }

    public required Guid CaseId { get; set; }
    public CourtCase? Case { get; set; }
}
