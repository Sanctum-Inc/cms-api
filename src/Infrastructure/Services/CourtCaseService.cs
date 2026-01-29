using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Update;
using Application.CourtCase.Queries.Get;
using Domain.CourtCases;
using ErrorOr;
using Infrastructure.Services.Base;
using MapsterMapper;
using MediatR;

namespace Infrastructure.Services;

public class CourtCaseService : BaseService<CourtCase, CourtCaseResult, AddCommand, UpdateCommand>, ICourtCaseService
{
    public CourtCaseService(
        IMapper mapper,
        ICourtCaseRepository courtCaseRepository,
        ISessionResolver sessionResolver) : base(courtCaseRepository, mapper, sessionResolver)
    {
    }

    public async Task<ErrorOr<IEnumerable<string>?>> GetCaseNumbers(CancellationToken cancellationToken)
    {
        var entity = await base.Get(cancellationToken);

        if (entity.IsError)
        {
            return entity.Errors;
        }

        var caseNumbers = entity.Value.Select(x => x.CaseNumber);

        return caseNumbers.ToErrorOr();
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }

    protected override ErrorOr<CourtCase> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        return new Domain.CourtCases.CourtCase
        {
            Id = Guid.NewGuid(),
            CaseNumber = command.CaseNumber,
            Location = command.Location,
            Plaintiff = command.Plaintiff,
            Defendant = command.Defendant,
            Status = command.Status,
            Type = command.Type,
            Outcome = command.Outcome,
            UserId = Guid.Parse(userId),
            IsPaid = false,
        };
    }

    protected override void MapFromUpdateCommand(CourtCase entity, UpdateCommand command)
    {
        entity.Defendant = command.Defendant;
        entity.Plaintiff = command.Plaintiff;
        entity.CaseNumber = command.CaseNumber;
        entity.Location = command.Location;
        entity.Status = command.Status;
        entity.Type = command.Type;
        entity.Outcome = command.Outcome;
    }
}
