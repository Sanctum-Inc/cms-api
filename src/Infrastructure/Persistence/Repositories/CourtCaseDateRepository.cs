using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.CourtCaseDates;

namespace Infrastructure.Persistence.Repositories;
public class CourtCaseDateRepository : BaseRepository<CourtCaseDate>, ICourtCaseDateRepository
{
    public CourtCaseDateRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context, sessionResolver)
    {
    }
}