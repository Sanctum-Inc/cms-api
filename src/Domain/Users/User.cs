using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using Domain.CourtCases;
using Domain.InvoiceItems;
using Domain.Lawyers;

namespace Domain.Users
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Surname { get; set; } = null!;

        [Required]
        public string MobileNumber { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string PasswordSalt { get; set; } = null!;

        [Required]
        public string RefreshToken { get; set; } = null!;

        // Relations
        public ICollection<CourtCase> CourtCases { get; set; } = [];
        public ICollection<Document> Documents { get; set; } = [];
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = [];
        public ICollection<Lawyer> Lawyers { get; set; } = [];
    }
}
