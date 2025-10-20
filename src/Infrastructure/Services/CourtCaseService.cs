using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Update;
using Application.CourtCase.Queries.Get;
using Domain.CourtCases;
using ErrorOr;
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

    public async Task<ErrorOr<bool>> Add(IRequest<ErrorOr<bool>> request, CancellationToken cancellationToken)
    {
        if (request is not AddCommand addCommand)
            return Error.Failure(description: "Invalid request type.");

        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        var user = await _userRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken);

        var courtCase = new Domain.CourtCases.CourtCase
        {
            Id = Guid.NewGuid(),
            CaseNumber = addCommand.CaseNumber,
            Location = addCommand.Location,
            Plaintiff = addCommand.Plaintiff,
            Defendant = addCommand.Defendant,
            Status = addCommand.Status,
            Type = addCommand.Type,
            Outcome = addCommand.Outcome,
            UserId = Guid.Parse(userId),
        };

        await _courtCaseRepository.AddAsync(courtCase, cancellationToken);
        await _courtCaseRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<bool>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var courtCase = await _courtCaseRepository
            .GetByIdAsync(id, cancellationToken);

        if (courtCase == null)
            return Error.NotFound(description: "Court case not found.");

        await _courtCaseRepository.DeleteAsync(courtCase, cancellationToken);
        await _courtCaseRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<IEnumerable<CourtCaseResult>>> Get(CancellationToken cancellationToken)
    {
        IEnumerable<CourtCase>? courtCases = await _courtCaseRepository
            .GetAll(cancellationToken);

        return _mapper.Map<IEnumerable<CourtCaseResult>>(courtCases).ToErrorOr();
    }

    public async Task<ErrorOr<CourtCaseResult?>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var courtCase = await _courtCaseRepository
            .GetByIdAndUserIdAsync(id, cancellationToken);

        return _mapper.Map<CourtCaseResult>(courtCase!);
    }

    public async Task<ErrorOr<bool>> Update<TRequest>(IRequest request, CancellationToken cancellationToken) where TRequest : IRequest<ErrorOr<bool>>
    {
        if (request is not UpdateCommand updateCommand)
            return Error.Failure(description: "Invalid request type.");

        var courtCase = await _courtCaseRepository
            .GetByIdAndUserIdAsync(new Guid(updateCommand.Id), cancellationToken);

        if (courtCase == null)
            return Error.Unexpected(description: "Court case not found.");

        courtCase.Defendant = updateCommand.Defendant;
        courtCase.Plaintiff = updateCommand.Plaintiff;
        courtCase.CaseNumber = updateCommand.CaseNumber;
        courtCase.Location = updateCommand.Location;
        courtCase.Status = updateCommand.Status;
        courtCase.Type = updateCommand.Type;
        courtCase.Outcome = updateCommand.Outcome;

        await _courtCaseRepository.UpdateAsync(courtCase, cancellationToken);
        await _courtCaseRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public Task<ErrorOr<bool>> Update(IRequest<ErrorOr<bool>> request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
