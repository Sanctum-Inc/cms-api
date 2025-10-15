using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.CourtCases;
using Domain.Users;

namespace Domain.Documents;

public class Document : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required string Path { get; set; }

    [Required]
    public required string FileName { get; set; }

    // Foreign Keys
    [Required]
    public Guid UserId { get; set; }
    public required User User { get; set; }

    [Required]
    public Guid CaseId { get; set; }
    public required CourtCase Case { get; set; }
}
