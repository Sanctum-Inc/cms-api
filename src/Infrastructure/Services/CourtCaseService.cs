using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.CourtCase.Queries.Get;
using Domain.CourtCases;
using MapsterMapper;

namespace Infrastructure.Services;

public class CourtCaseService : ICourtCaseService
{
    private readonly ICourtCaseRepository _courtCaseRepository;
    private readonly ISessionResolver _currentUserService;
    private readonly IMapper _mapper;

    public CourtCaseService(
        ICourtCaseRepository courtCaseRepository,
        ISessionResolver sessionResolver,
        IMapper mapper)
    {
        _courtCaseRepository = courtCaseRepository;
        _currentUserService = sessionResolver;
        _mapper = mapper;
    }

    public async Task<GetCourtCaseResult> Get(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User is not authenticated.");

        var courtCases = await _courtCaseRepository
            .GetByUserIdAsync(Guid.Parse(userId), cancellationToken);

        return _mapper.Map<GetCourtCaseResult>(courtCases);
    }

    public async Task<CourtCaseResult> GetById(string id, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User is not authenticated.");

        var courtCase = await _courtCaseRepository
            .GetByIdAndUserIdAsync(Guid.Parse(id), Guid.Parse(userId), cancellationToken);

        return _mapper.Map<CourtCaseResult>(courtCase);
    }
}
