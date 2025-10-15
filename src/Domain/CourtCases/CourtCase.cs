﻿using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.CourtCaseDates;
using Domain.Documents;
using Domain.InvoiceItems;
using Domain.Lawyers;
using Domain.Users;

namespace Domain.CourtCases;
public class CourtCase : AuditableEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public required string CaseNumber { get; set; }
    [Required]
    public required string Location { get; set; }
    [Required]
    public required string Plaintiff { get; set; }
    [Required]
    public required string Defendant { get; set; }
    [Required]
    public required string Status { get; set; }
    public string? Type { get; set; }
    public string? Outcome { get; set; }
    [Required]
    public Guid UserId { get; set; }
    public required User User { get; set; }
    public List<CourtCaseDate> CourtCaseDates { get; set; } = [];
    public List<Document> Documents { get; set; } = [];
    public List<InvoiceItem> InvoiceItems { get; set; } = [];
    public List<Lawyer> Lawyers { get; set; } = [];
}
