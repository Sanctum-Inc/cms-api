using Application.Common.Models;
using Application.CourtCase.Queries.Get;
using Domain.CourtCases;
using Mapster;

namespace testfs;
public static class MappingConfig
{
    public static void RegisterMapping()
    {
        // Map single entity to result DTO
        TypeAdapterConfig<CourtCase, CourtCaseResult>.NewConfig();

        // Map collection of domain objects to GetCourtCaseResult
        TypeAdapterConfig<IEnumerable<CourtCase>, GetCourtCaseResult>
            .NewConfig()
            .Map(dest => dest.CourtCases, src => src);
    }
}
