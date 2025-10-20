using Application.Common.Models;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Update;
using Application.CourtCase.Queries.Get;
using ErrorOr;
using MediatR;

namespace Application.Common.Interfaces.Services;

/// <summary>
/// Provides a set of operations for managing and retrieving court case data.
/// </summary>
public interface ICourtCaseService : IBaseService<CourtCaseResult>
{
}
