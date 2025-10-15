using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Update;
using Application.CourtCase.Queries.Get;
using Domain.CourtCases;
using ErrorOr;
using Infrastructure.Common;
using MapsterMapper;
using MediatR;

namespace Infrastructure.Services;

public class CourtCaseService : ICourtCaseService
{
    private readonly ICourtCaseRepository _courtCaseRepository;
    private readonly ISessionResolver _currentUserService;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public CourtCaseService(
        ICourtCaseRepository courtCaseRepository,
        ISessionResolver sessionResolver,
        IMapper mapper,
        IUserRepository userRepository)
    {
        _courtCaseRepository = courtCaseRepository;
        _currentUserService = sessionResolver;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<bool>> Add(AddCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        var user = await _userRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken);

        var courtCase = new Domain.CourtCases.CourtCase
        {
            Id = Guid.NewGuid(),
            CaseNumber = request.CaseNumber,
            Location = request.Location,
            Plaintiff = request.Plaintiff,
            Defendant = request.Defendant,
            Status = request.Status,
            Type = request.Type,
            Outcome = request.Outcome,
            UserId = Guid.Parse(userId),
            User = user!
        };

        await _courtCaseRepository.AddAsync(courtCase, cancellationToken);
        await _courtCaseRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<bool>> Delete(string id, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User is not authenticated.");

        var courtCase = await _courtCaseRepository
            .GetByIdAndUserIdAsync(new Guid(id), Guid.Parse(userId), cancellationToken);

        if (courtCase == null)
            return Error.NotFound(description: "Court case not found.");

        await _courtCaseRepository.DeleteAsync(courtCase, cancellationToken);
        await _courtCaseRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<GetCourtCaseResult> Get(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User is not authenticated.");

        IEnumerable<CourtCase>? courtCases = await _courtCaseRepository
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

        return _mapper.Map<CourtCaseResult>(courtCase!);
    }

    public async Task<ErrorOr<bool>> Update(UpdateCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User is not authenticated.");

        var courtCase = await _courtCaseRepository
            .GetByIdAndUserIdAsync(new Guid(request.Id), Guid.Parse(userId), cancellationToken);

        if (courtCase == null)
            return Error.NotFound(description: "Court case not found.");

        courtCase.Defendant = request.Defendant;
        courtCase.Plaintiff = request.Plaintiff;
        courtCase.CaseNumber = request.CaseNumber;
        courtCase.Location = request.Location;
        courtCase.Status = request.Status;
        courtCase.Type = request.Type;
        courtCase.Outcome = request.Outcome;

        await _courtCaseRepository.UpdateAsync(courtCase, cancellationToken);
        await _courtCaseRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
