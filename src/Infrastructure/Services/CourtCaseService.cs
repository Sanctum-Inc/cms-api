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
