using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Firm.Requests;
public record UpdateFirmRequest(
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
