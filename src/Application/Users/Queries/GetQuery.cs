using ErrorOr;
using MediatR;

namespace Application.Users.Queries;
public record GetQuery(string Id) : IRequest<ErrorOr<UserResult>>;