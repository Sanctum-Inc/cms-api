using System.Runtime.InteropServices.JavaScript;
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
            Description =  command.Description,
            CaseId = command.CaseId,
            Type = command.Type,
            Case = null!,
            UserId = new Guid(userId),
            IsComplete =  false,
            IsCanceled = false,
        };
    }

    protected override void MapFromUpdateCommand(CourtCaseDate entity, UpdateCommand command)
    {
        entity.Date = command.Date;
        entity.Title = command.Title;
        entity.Description = command.Description;
        entity.CaseId = command.CaseId;
        entity.IsComplete = command.IsComplete;
        entity.IsCanceled = command.IsCancelled;
    }

    public async Task<ErrorOr<CourtCaseDateResult>?> GetCourtCaseDateInformation(CancellationToken cancellationToken)
    {
        var result = await _courtCaseDateRepository.GetAll(cancellationToken);
        var courtCaseDates = result.ToList();

        if (!courtCaseDates.Any())
        {
            return Error.NotFound("CourtCaseDates.Empty", "No court case dates found");
        }

        var overdueItems = GetOverDueItems(courtCaseDates);
        var completionRate = GetCompletionRate(courtCaseDates);
        var upcomingEvents = GetUpcomingEvents(courtCaseDates);
        var changeFromLastMonth = GetChangeFromLastMonth(courtCaseDates);

        return new CourtCaseDateResult(
                overdueItems,
                completionRate,
                upcomingEvents,
                changeFromLastMonth,
                FindDeadlineCase(courtCaseDates),
                courtCaseDates
                    .Select(x => new CourtCaseDateItem(
                        x.Id,
                        x.Date,
                        x.Title,
                        x.Case.CaseNumber,
                        x.CaseId,
                        x.Type,
                        $"{x.Case.Plaintiff} vs {x.Case.Defendant}",
                        x.Description,
                        GetCourtCaseDateStatus(x.Date)
                    ))
                    .OrderBy(x => x.Date)
            )
            .ToErrorOr();

    }

    public async Task<ErrorOr<bool>> SetToCancelled(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _courtCaseDateRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            return Error.NotFound("CourtCaseDates.Empty", "No court case dates found");
        }

        entity.IsCanceled = true;

        await _courtCaseDateRepository.UpdateAsync(entity, cancellationToken);
        await _courtCaseDateRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<bool>> SetToComplete(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _courtCaseDateRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            return Error.NotFound("CourtCaseDates.Empty", "No court case dates found");
        }

        entity.IsComplete = true;

        await _courtCaseDateRepository.UpdateAsync(entity, cancellationToken);
        await _courtCaseDateRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    private float GetChangeFromLastMonth(List<CourtCaseDate> courtCaseDates)
    {
        var now = DateTime.Now;

        var currentMonthStart = new DateTime(now.Year, now.Month, 1);
        var previousMonthStart = currentMonthStart.AddMonths(-1);

        var currentMonthCount = courtCaseDates.Count(x =>
        {
            var date = DateTime.Parse(x.Date);
            return date >= currentMonthStart && date < currentMonthStart.AddMonths(1);
        });

        var previousMonthCount = courtCaseDates.Count(x =>
        {
            var date = DateTime.Parse(x.Date);
            return date >= previousMonthStart && date < currentMonthStart;
        });

        if (previousMonthCount == 0)
            return currentMonthCount > 0 ? 100f : 0f;

        return ((float)(currentMonthCount - previousMonthCount) / previousMonthCount) * 100f;
    }

    private int GetUpcomingEvents(IList<CourtCaseDate> result)
    {
        return result
            .Count(x => DateTime.Parse(x.Date) > DateTime.Now && DateTime.Parse(x.Date) < DateTime.Now.AddDays(30));
    }

    private int GetCompletionRate(IList<CourtCaseDate> result)
    {
        var completedCases = result
            .Count(x => x.IsComplete);

        var totalCases = result.Count();

        return completedCases /  totalCases;
    }

    private int GetOverDueItems(IEnumerable<CourtCaseDate> result)
    {
        return result
            .Count(x => GetCourtCaseDateStatus(x.Date) == "Overdue");
    }

    private CourtCaseDateItem? FindDeadlineCase(IList<CourtCaseDate>? result)
    {
        return result?
            .Select(x => new CourtCaseDateItem(
                x.Id,
                x.Date,
                x.Title,
                x.Case.CaseNumber,
                x.CaseId,
                x.Type,
                $"{x.Case.Plaintiff} vs {x.Case.Defendant}",
                x.Description,
                GetCourtCaseDateStatus(x.Date)
            ))
            .OrderBy(x => x.Date)
            .FirstOrDefault();
    }


    private string GetCourtCaseDateStatus(string dateString)
    {
        var date = DateTime.Parse(dateString);
        if (date.Day == DateTime.Now.Day && date.Month == DateTime.Now.Month && date.Year == DateTime.Now.Year)
        {
            return "DueToday";
        }
        else if (date < DateTime.Now)
        {
            return "Overdue";
        }
        else
        {
            return "Upcoming";
        }
    }
}
