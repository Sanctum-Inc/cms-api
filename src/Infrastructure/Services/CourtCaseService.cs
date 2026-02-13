using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Update;
using Domain.CourtCaseDates;
using Domain.CourtCases;
using ErrorOr;
using MapsterMapper;

namespace Infrastructure.Services;

public class CourtCaseService : BaseService<CourtCase, CourtCaseResult, AddCommand, UpdateCommand>, ICourtCaseService
{
    private readonly ICourtCaseRepository _courtCaseRepository;
    public CourtCaseService(
        IMapper mapper,
        ICourtCaseRepository courtCaseRepository,
        ISessionResolver sessionResolver) : base(courtCaseRepository, mapper, sessionResolver)
    {
        _courtCaseRepository = courtCaseRepository;
    }

    public async Task<ErrorOr<IEnumerable<CourtCaseNumberResult>?>> GetCaseNumbers(CancellationToken cancellationToken)
    {
        var entity = await base.Get(cancellationToken);

        if (entity.IsError)
        {
            return entity.Errors;
        }

        var caseNumbers = entity.Value.Select(x => new CourtCaseNumberResult(x.Id.ToString(), x.CaseNumber));

        return caseNumbers.ToErrorOr();
    }

    public async Task<ErrorOr<CourtCaseInformationResult>> GetCourtCaseInformationById(Guid id, CancellationToken cancellationToken)
    {
        var courtcase = await _courtCaseRepository.GetByIdAndUserIdAsync(id, cancellationToken);

        if (courtcase is null)
        {
            Error.NotFound("CourtCase.NotFound", "CourtCase not found.");
        }

        return new CourtCaseInformationResult()
        {
            CaseId = courtcase.Id,
            CaseNumber = courtcase.CaseNumber,
            Location = courtcase.Location,
            Plaintiff = courtcase.Plaintiff,
            Defendant = courtcase.Defendant,
            CaseType = courtcase.Type,
            CaseOutcomes = courtcase.Outcome,
            CreatedAt = courtcase.Created,
            LastModified = courtcase.LastModified ?? courtcase.Created,
            Dates = courtcase.CourtCaseDates
                .Select(x => new CourtCaseInformationQueryDatesResult()
                {
                    Date = x.Created,
                    DateType = x.Type,
                    Description = x.Description
                })
                .ToList(),
            Documents = courtcase.Documents
                .Select(x => new CourtCaseInformationQueryDocumentsResult()
                {
                    DateFiled = x.Created,
                    FileType = x.ContentType,
                    Title = x.FileName
                })
                .ToList(),
            Invoices = courtcase.Invoices
                .Select(x => new CourtCaseInformationQueryInvoicesResult()
                {
                    InvoiceNumber = x.InvoiceNumber,
                    Amount = x.Items.Sum(x => x.CostPerHour * x.Hours),
                    Status = x.Status,
                })
                .ToList(),
            Lawyers = courtcase.Lawyers
                .Select(x => new CourtCaseInformationQueryLawyersResult()
                {
                    Name = x.Name,
                    Email = x.Email,
                    MobileNumber = x.MobileNumber,
                })
                .ToList()
        };
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }

    protected override ErrorOr<CourtCase> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Error.Unauthorized(description: "User is not authenticated.");
        }

        return new CourtCase
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
            IsPaid = false
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

    private static string GetNextRelevantDate(List<CourtCaseDate> courtCaseDates)
    {
        var mostRecentDate = courtCaseDates
            .OrderByDescending(x => x.Date)
            .FirstOrDefault(x => x is { IsCanceled: false, IsComplete: false });

        return mostRecentDate?.Date.ToString() ?? "";
    }
}
