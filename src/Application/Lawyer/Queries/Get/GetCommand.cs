using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Queries.Get;

public record GetCommand : IRequest<ErrorOr<IEnumerable<LawyerResult>>>;
