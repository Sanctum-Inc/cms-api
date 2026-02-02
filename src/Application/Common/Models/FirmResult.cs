namespace Application.Common.Models;

public record FirmResult
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Telephone { get; set; }
    public required string Fax { get; set; }
    public required string Mobile { get; set; }
    public required string Email { get; set; }
    public required string AttorneyAdmissionDate { get; set; }
    public required string AdvocateAdmissionDate { get; set; }
    public required string AccountName { get; set; }
    public required string Bank { get; set; }
    public required string BranchCode { get; set; }
    public required string AccountNumber { get; set; }
}
