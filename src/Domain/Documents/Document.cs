using System.ComponentModel.DataAnnotations;
using Domain.CourtCases;
using Domain.Users;

namespace Domain.Documents
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Path { get; set; } = null!;

        [Required]
        public string FileName { get; set; } = null!;

        public DateTime DateCreated { get; set; }

        // Foreign Keys
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public Guid CaseId { get; set; }
        public CourtCase Case { get; set; } = null!;
    }
}
