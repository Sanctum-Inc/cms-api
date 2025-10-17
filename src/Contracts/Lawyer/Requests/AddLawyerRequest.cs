using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Lawyer.Requests;
public record AddLawyerRequest(
    string Email,
    string Name,
    string Surname,
    string MobileNumber,
    string Specialty
);
