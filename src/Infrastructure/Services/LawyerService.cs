using System.Globalization;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Lawyer.Commands.Add;
using Application.Lawyer.Commands.Update;
using Application.Users.Queries;
using Domain.CourtCases;
using Domain.Invoices;
using Domain.Lawyers;
using Domain.Users;
using ErrorOr;
using Infrastructure.Common;
using MapsterMapper;

namespace Infrastructure.Services;

public class LawyerService : BaseService<Lawyer, LawyerResult, AddCommand, UpdateCommand>, ILawyerService
{
    private readonly ILawyerRepository _lawyerRepository;
    private readonly IUserRepository _userRepository;

    public LawyerService(
        IUserRepository userRepository,
        ILawyerRepository lawyerLawyerRepository,
        ISessionResolver sessionResolver,
        IMapper mapper) : base(lawyerLawyerRepository, mapper, sessionResolver)
    {
        _lawyerRepository = lawyerLawyerRepository;
        _userRepository = userRepository;
    }

    protected override ErrorOr<Lawyer> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Error.Unauthorized(description: "User is not authenticated.");
        }

        return new Lawyer
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Surname = command.Surname,
            Specialty = command.Specialty,
            MobileNumber = command.MobileNumber,
            Email = command.Email,
            UserId = Guid.Parse(userId)
        };
    }

    protected override void MapFromUpdateCommand(Lawyer entity, UpdateCommand command)
    {
        entity.Name = command.Name;
        entity.Surname = command.Surname;
        entity.Specialty = command.Specialty;
        entity.MobileNumber = command.MobileNumber;
        entity.Email = command.Email;
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }

    public async override Task<ErrorOr<IEnumerable<LawyerResult>>> Get(CancellationToken cancellationToken)
    {
        var result = await _lawyerRepository.GetAll(cancellationToken);

        return result
            .Select(x => new LawyerResult()
            {
                Email = x.Email,
                MobileNumber = x.MobileNumber,
                Name = x.Name,
                Speciality = x.Specialty,
                Surname = x.Surname,
                Id = x.Id,
                TotalCases = x.CourtCases.Count
            })
            .ToList();
    }

    public async Task<ErrorOr<LawyerReportResult>> GetLawyerReport(Guid id, CancellationToken cancellationToken)
    {
        var lawyer = await _lawyerRepository.GetLawyerReportInformation(id, cancellationToken);
        if (lawyer == null)
            return Error.NotFound("Lawyer.NotFound","Lawyer not found.");

        return new LawyerReportResult(
            GetLawyerReportCardsInformation(lawyer, cancellationToken),
            GetLawyerReportInvoiceInformation(lawyer,cancellationToken),
            GetLawyerReportCaseDistribution(lawyer, cancellationToken),
            GetLawyerReportUpcomingDeadlines(lawyer, cancellationToken),
            GetLawyerReportActivityLog(lawyer, cancellationToken),
            new LawyerResult()
            {
                Id =  lawyer.Id,
                Email = lawyer.Email,
                MobileNumber = lawyer.MobileNumber,
                Name = lawyer.Name,
                Speciality = lawyer.Specialty,
                Surname = lawyer.Surname,
                TotalCases = lawyer.CourtCases.Count,
            });
    }


    private IList<LawyerReportActivityLogResult> GetLawyerReportActivityLog(Lawyer lawyer , CancellationToken cancellationToken)
    {
        var courtCases = lawyer.CourtCases
            .Select(x => new LawyerReportActivityLogResult(
                x.CaseNumber,
                DateTimeFormater.GetTimeAgo(x.Created)))
            .ToList();

        var dates = lawyer.CourtCaseDates
            .Select(x => new LawyerReportActivityLogResult(
                x.Title,
                DateTimeFormater.GetTimeAgo(x.Created)))
            .ToList();

        var invoices = lawyer.Invoices
            .Select(x => new LawyerReportActivityLogResult(
                x.InvoiceNumber,
                DateTimeFormater.GetTimeAgo(x.Created)))
            .ToList();


        courtCases
            .AddRange(dates);
        courtCases
            .AddRange(invoices);

        return courtCases
            .OrderBy(x => x.HoursAgo)
            .Take(10)
            .ToList();
    }

    private IList<LawyerReportUpcomingDeadlinesResult> GetLawyerReportUpcomingDeadlines(Lawyer lawyer, CancellationToken cancellationToken)
    {
        return lawyer.CourtCases
            .SelectMany(x => x.CourtCaseDates)
            .Select(x => new LawyerReportUpcomingDeadlinesResult(
                x.Title,
                x.CaseId.ToString(),
                x.Date))
            .Where(x => DateTime.Parse(x.Date) > DateTime.Now)
            .ToList();
    }

    private IList<LawyerReportCaseDistributionResult> GetLawyerReportCaseDistribution(Lawyer lawyer, CancellationToken cancellationToken)
    {
        return lawyer.CourtCases
            .GroupBy(x => x.Type)
            .Select(group => new LawyerReportCaseDistributionResult(
                group.Key,
                group.Count()))
            .OrderByDescending(x => x.TotalAmount)
            .Take(4)
            .ToList();
    }

    private IList<LawyerReportInvoicesResult> GetLawyerReportInvoiceInformation(Lawyer lawyer, CancellationToken cancellationToken)
    {
        return lawyer.CourtCases
            .SelectMany(x => x.Invoices)
            .Select(x => new LawyerReportInvoicesResult(
                x.InvoiceNumber,
                $"{x.Case.Plaintiff} vs {x.Case.Defendant}",
                x.Case.CaseNumber,
                x.Items.Sum(x => x.CostPerHour * x.Hours),
                x.Status))
            .ToList();
    }

    private IList<LawyerReportCardsResult> GetLawyerReportCardsInformation(
    Lawyer lawyer,
    CancellationToken cancellationToken)
    {
        IList<LawyerReportCardsResult> lawyerReportCardsResults = [];

        var now = DateTime.UtcNow;

        var startOfThisMonth = new DateTime(now.Year, now.Month, 1);
        var startOfLastMonth = startOfThisMonth.AddMonths(-1);
        var startOfMonthBeforeLast = startOfThisMonth.AddMonths(-2);

        #region 💰 BILLING

        decimal totalBillings = lawyer.CourtCases
            .SelectMany(x => x.Invoices)
            .SelectMany(x => x.Items)
            .Sum(x => x.CostPerHour * x.Hours);

        decimal totalPendingBilling = lawyer.CourtCases
            .SelectMany(x => x.Invoices)
            .Where(x => x.Status != InvoiceStatus.PAID)
            .SelectMany(x => x.Items)
            .Sum(x => x.CostPerHour * x.Hours);

        decimal billedLastMonth = lawyer.CourtCases
            .SelectMany(x => x.Invoices)
            .SelectMany(x => x.Items)
            .Where(x =>
                x.DateOfService >= startOfLastMonth &&
                x.DateOfService < startOfThisMonth)
            .Sum(x => x.CostPerHour * x.Hours);

        decimal billedThisMonth = lawyer.CourtCases
            .SelectMany(x => x.Invoices)
            .SelectMany(x => x.Items)
            .Where(x =>
                x.DateOfService >= startOfThisMonth &&
                x.DateOfService < startOfThisMonth.AddMonths(1))
            .Sum(x => x.CostPerHour * x.Hours);

        decimal billingPercentage = 0;
        if (billedLastMonth != 0)
        {
            billingPercentage =
                ((billedThisMonth - billedLastMonth) / billedLastMonth) * 100;
        }

        lawyerReportCardsResults.Add(new(
            totalBillings.ToString(CultureInfo.InvariantCulture),
            totalPendingBilling.ToString(CultureInfo.InvariantCulture),
            billingPercentage.ToString(CultureInfo.InvariantCulture)));

        #endregion

        #region ⚖️ CASES

        int activeCourtCaseCount = lawyer.CourtCases
            .Count(x => x.Status != CourtCaseStatus.Cancelled &&
                        x.Status != CourtCaseStatus.Closed);
        int closedLastMonth = lawyer.CourtCases
            .Count(x =>
                x.Status == CourtCaseStatus.Closed &&
                x.LastModified >= startOfLastMonth &&
                x.LastModified < startOfThisMonth);

        int closedThisMonth = lawyer.CourtCases
            .Count(x =>
                x.Status == CourtCaseStatus.Closed &&
                x.LastModified >= startOfThisMonth &&
                x.LastModified < startOfThisMonth.AddMonths(1));

        decimal casePercentageIncrease = 0;
        if (closedLastMonth != 0)
        {
            casePercentageIncrease =
                ((closedThisMonth - closedLastMonth) / (decimal)closedLastMonth) * 100;
        }

        lawyerReportCardsResults.Add(new(
            activeCourtCaseCount.ToString(CultureInfo.InvariantCulture),
            closedThisMonth.ToString(CultureInfo.InvariantCulture),
            casePercentageIncrease.ToString(CultureInfo.InvariantCulture)));

        #endregion

        #region 📅 EFFICIENCY RATE

        int completedLastMonth = lawyer.CourtCases
            .SelectMany(x => x.CourtCaseDates)
            .Count(x =>
                x.IsComplete &&
                x.Created >= startOfLastMonth &&
                x.Created < startOfThisMonth);

        int deadlinesLastMonth = lawyer.CourtCases
            .SelectMany(x => x.CourtCaseDates)
            .Count(x =>
                x.Created >= startOfLastMonth &&
                x.Created < startOfThisMonth);

        int completedThisMonth = lawyer.CourtCases
            .SelectMany(x => x.CourtCaseDates)
            .Count(x =>
                x.IsComplete &&
                x.Created >= startOfThisMonth &&
                x.Created < startOfThisMonth.AddMonths(1));

        int deadlinesThisMonth = lawyer.CourtCases
            .SelectMany(x => x.CourtCaseDates)
            .Count(x =>
                x.Created >= startOfThisMonth &&
                x.Created < startOfThisMonth.AddMonths(1));

        decimal lastMonthEfficiencyRate = 0;
        if (deadlinesLastMonth != 0)
            lastMonthEfficiencyRate =
                (completedLastMonth / (decimal)deadlinesLastMonth) * 100;

        decimal thisMonthEfficiencyRate = 0;
        if (deadlinesThisMonth != 0)
            thisMonthEfficiencyRate =
                (completedThisMonth / (decimal)deadlinesThisMonth) * 100;

        decimal efficiencyIncrease = thisMonthEfficiencyRate - lastMonthEfficiencyRate;

        lawyerReportCardsResults.Add(new(
            thisMonthEfficiencyRate.ToString(CultureInfo.InvariantCulture),
            completedThisMonth.ToString(CultureInfo.InvariantCulture),
            efficiencyIncrease.ToString(CultureInfo.InvariantCulture)));

        #endregion

        #region 🏛 AVERAGE SETTLEMENT PER CASE

        // Assuming CourtCase has SettlementAmount decimal property
        decimal settlementLastMonthTotal = lawyer.CourtCases
            .Where(x =>
                x.Status == CourtCaseStatus.Closed &&
                x.LastModified >= startOfLastMonth &&
                x.LastModified < startOfThisMonth)
            .SelectMany(x => x.Invoices)
            .SelectMany(x => x.Items)
            .Sum(x => x.CostPerHour * x.Hours);

        decimal settlementThisMonthTotal = lawyer.CourtCases
            .Where(x =>
                x.Status == CourtCaseStatus.Closed &&
                x.LastModified >= startOfThisMonth &&
                x.LastModified < startOfThisMonth.AddMonths(1))
            .SelectMany(x => x.Invoices)
            .SelectMany(x => x.Items)
            .Sum(x => x.CostPerHour * x.Hours);

        decimal avgSettlementLastMonth = 0;
        if (closedLastMonth != 0)
            avgSettlementLastMonth =
                settlementLastMonthTotal / closedLastMonth;

        decimal avgSettlementThisMonth = 0;
        if (closedThisMonth != 0)
            avgSettlementThisMonth =
                settlementThisMonthTotal / closedThisMonth;

        decimal avgSettlementIncrease = 0;
        if (avgSettlementLastMonth != 0)
            avgSettlementIncrease =
                ((avgSettlementThisMonth - avgSettlementLastMonth)
                    / avgSettlementLastMonth) * 100;

        lawyerReportCardsResults.Add(new(
            avgSettlementThisMonth.ToString(CultureInfo.InvariantCulture),
            settlementThisMonthTotal.ToString(CultureInfo.InvariantCulture),
            avgSettlementIncrease.ToString(CultureInfo.InvariantCulture)));

        #endregion

        return lawyerReportCardsResults;
    }
}
