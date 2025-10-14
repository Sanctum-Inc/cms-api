using System.ComponentModel.DataAnnotations;
using Domain.CourtCases;
using Domain.Users;

namespace Domain.InvoiceItems
{
    public class InvoiceItem
    {
        [Key]
        public Guid Id { get; set; }

        public string? InvoiceNumber { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public DateTime Date { get; set; }
        public int Hours { get; set; }
        public float CostPerHour { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public Guid CaseId { get; set; }
        public CourtCase Case { get; set; } = null!;
    }
}
