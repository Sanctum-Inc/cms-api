using Domain.Common;
using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Documents;
using Domain.Emails;
using Domain.Firms;
using Domain.Invoices;
using Domain.Lawyers;

namespace Domain.Users;

public class User : AuditableEntity
{
    public required string Email { get; set; }

    public required string Name { get; set; }

    public required string Surname { get; set; }

    public required string MobileNumber { get; set; }

    public required string PasswordHash { get; set; }

    public required string PasswordSalt { get; set; }

    public required UserRole Role { get; set; } = UserRole.FirmUser;

    public required bool IsEmailVerified { get; set; }

    public DateTime? LastVerificationEmailSentAt { get; set; }
    public int VerificationEmailSentCount { get; set; }
    public int EmailVerificationTokenVersion { get; set; }

    // Relations
    public required Guid FirmId { get; set; }
    public Firm? Firm { get; set; }

    public List<CourtCase> CourtCases { get; set; } = [];
    public List<CourtCaseDate> CourtCasesDates { get; set; } = [];
    public List<Document> Documents { get; set; } = [];
    public List<Invoice> Invoices { get; set; } = [];
    public List<Lawyer> Lawyers { get; set; } = []; // Lawyers created by this user
    public List<Email> Emails { get; set; } = []; // Lawyers created by this user
}
