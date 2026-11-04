using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Email.Queries.Get;

public record GetEmailQuery : IRequest<ErrorOr<IEnumerable<EmailResult>>>;
