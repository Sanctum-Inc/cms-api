using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;

namespace Infrastructure.Persistence.Repositories;
public class LawyerRepository : BaseRepository<Domain.Lawyers.Lawyer> ,ILawyerRepository
{
    public LawyerRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context, sessionResolver)
    {
    }
}
