using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Email.Queries.GetById;

public record GetByIdQuery(Guid Id) : IRequest<ErrorOr<EmailResult>>;
