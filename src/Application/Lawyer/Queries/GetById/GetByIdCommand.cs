using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Lawyer.Queries.GetById;

public record GetByIdCommand(Guid Id) : IRequest<ErrorOr<LawyerResult?>>;
