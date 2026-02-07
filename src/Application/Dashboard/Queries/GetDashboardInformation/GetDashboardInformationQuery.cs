using Application.Common.Models;
using MediatR;
using ErrorOr;

namespace Application.Dashboard.Queries.GetDashboardInformation;

public record GetDashboardInformationQuery() : IRequest<ErrorOr<DashBoardResult>>;
