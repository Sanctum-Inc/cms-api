using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.CourtCaseDates.Commands.Add;
using Application.CourtCaseDates.Commands.Update;
using Domain.CourtCaseDates;
using ErrorOr;
using MapsterMapper;

namespace Infrastructure.Services;

public class CourtCaseDatesService : BaseService<CourtCaseDate, CourtCaseDateResult, AddCommand, UpdateCommand>,
    ICourtCaseDatesService
{
    public readonly ICourtCaseDateRepository _courtCaseDateRepository;

    public CourtCaseDatesService(
        IMapper mapper,
        ICourtCaseDateRepository courtCaseDateRepository,
        ISessionResolver sessionResolver) : base(courtCaseDateRepository, mapper, sessionResolver)
    {
        _courtCaseDateRepository = courtCaseDateRepository;
    }

    public override async Task<ErrorOr<IEnumerable<CourtCaseDateResult>>> Get(CancellationToken cancellationToken)
    {
        var result = await _courtCaseDateRepository.GetAll(cancellationToken);

        return result.Select(x => new CourtCaseDateResult(
            x.Id,
            x.Date,
            x.Title,
            x.Case.CaseNumber,
            x.CaseId,
            x.Case.Type,
            Defendent: x.Case.Defendant,
            Platiniff: x.Case.Plaintiff)
        ).ToErrorOr(); // ToDo: change this to subtitle and add it to database and frontend
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }

    protected override ErrorOr<CourtCaseDate> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Error.Unauthorized(description: "User is not authenticated.");
        }

        return new CourtCaseDate
        {
            Id = Guid.NewGuid(),
            Date = command.Date,
            Title = command.Title,
            CaseId = command.CaseId,
            Type = command.Type,
            Case = null!,
            UserId = new Guid(userId)
        };
    }

    protected override void MapFromUpdateCommand(CourtCaseDate entity, UpdateCommand command)
    {
        entity.Date = command.Date;
        entity.Title = command.Title;
        entity.CaseId = command.CaseId;
    }
}
