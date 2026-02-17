using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Queries.GetReportInformation;

public record GetReportInformationQuery(
    Guid LawyerId) : IRequest<ErrorOr<LawyerReportResult>>;
