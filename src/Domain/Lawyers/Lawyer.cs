using System.ComponentModel.DataAnnotations;
using Domain.CourtCaseDates;
using Domain.CourtCases;
using Domain.Users;

namespace Domain.Lawyers
{
    public class Lawyer
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
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Relations
        public ICollection<CourtCase> CourtCases { get; set; } = [];
        public ICollection<CourtCaseDate> CourtCaseDates { get; set; } = [];
    }
}
