using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.Users.Queries.GetQuery;

public record GetUsersQuery(string Id) : IRequest<ErrorOr<UserResult>>;
