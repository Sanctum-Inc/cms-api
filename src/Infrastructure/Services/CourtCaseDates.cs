using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.CourtCaseDates.Commands.Add;
using Application.CourtCaseDates.Commands.Update;
using Domain.CourtCaseDates;
using ErrorOr;
using Infrastructure.Services.Base;
using MapsterMapper;

namespace Infrastructure.Services;

public class CourtCaseDatesService : BaseService<CourtCaseDate, CourtCaseDateResult, AddCommand, UpdateCommand>, ICourtCaseDatesService
{
    public CourtCaseDatesService(
        IMapper mapper,
        ICourtCaseDateRepository courtCaseDateRepository,
        ISessionResolver sessionResolver) : base(courtCaseDateRepository, mapper, sessionResolver)
    {
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }

    protected override ErrorOr<CourtCaseDate> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        return new CourtCaseDate
        {
            Id = Guid.NewGuid(),
            Date = command.Date,
            Title = command.Title,
            CaseId = command.CaseId,
            Case = null!,
            UserId = new Guid(userId),
        };
    }

    protected override void MapFromUpdateCommand(CourtCaseDate entity, UpdateCommand command)
    {
        throw new NotImplementedException();
    }
}
