using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using Domain.CourtCaseDates;
using Domain.InvoiceItems;
using Domain.Lawyers;
using Domain.Users;

namespace Domain.CourtCases
{
    public class CourtCase
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required string CaseNumber
        {
            get; set;
        }

        [Required]
        public required string Location
        {
            get; set;
        }

        [Required]
        public required string Plaintiff
        {
            get; set;
        }

        [Required]
        public required string Defendant
        {
            get; set;
        }

        [Required]
        public required string Status
        {
            get; set;
        }

        public string? Type { get; set; }
        public string? Outcome { get; set; }
        public DateTime DateCreated { get; set; }

        // Foreign Key to User
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Relations
        public ICollection<CourtCaseDate> CourtCaseDates { get; set; } = [];
        public ICollection<Document> Documents { get; set; } = [];
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = [];

        // Many-to-Many with Lawyer
        public ICollection<Lawyer> Lawyers { get; set; } = [];
    }
}
