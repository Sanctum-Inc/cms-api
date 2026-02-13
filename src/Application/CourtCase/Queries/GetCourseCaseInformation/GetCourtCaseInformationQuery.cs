using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.GetCourseCaseInformation;

public record GetCourtCaseInformationQuery(
    Guid Id) : IRequest<ErrorOr<CourtCaseInformationResult>>;
