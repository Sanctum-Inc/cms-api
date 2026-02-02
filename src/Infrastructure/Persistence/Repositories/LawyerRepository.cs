using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Session;
using Domain.Lawyers;

namespace Infrastructure.Persistence.Repositories;

public class LawyerRepository : BaseRepository<Lawyer>, ILawyerRepository
{
    public LawyerRepository(IApplicationDBContext context, ISessionResolver sessionResolver) : base(context,
        sessionResolver)
    {
    }
}
