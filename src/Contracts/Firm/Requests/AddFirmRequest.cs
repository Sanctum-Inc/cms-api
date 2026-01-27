namespace Contracts.Firm.Requests;
public record AddFirmRequest(
    string Name,
    string Address,
    string Telephone,
    string Fax,
    string Mobile,
    string Email,
    string AttorneyAdmissionDate,
    string AdvocateAdmissionDate,
    string AccountName,
    string Bank,
    string BranchCode,
    string AccountNumber
);
