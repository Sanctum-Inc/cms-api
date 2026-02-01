using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Email.Queries.Get;
public record GetQuery() : IRequest<ErrorOr<IEnumerable<EmailResult>>>;
