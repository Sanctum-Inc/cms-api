using Contracts.Common;

namespace Contracts.CourtCases.Responses;

public class GetCourtCasesResponse()
{
    public List<CourtCasesResponse>? CourtCases { get; set; }
}