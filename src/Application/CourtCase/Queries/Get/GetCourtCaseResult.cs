using Application.Common.Models;
using ErrorOr;

namespace Application.CourtCase.Queries.Get;
public class GetCourtCaseResult
{
    public List<CourtCaseResult>? CourtCases { get; set; }
}
