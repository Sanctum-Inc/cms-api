using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Document.Commands.Add;
public record AddCommand(
    IFormFile File,
    string Name,
    string CaseId) :IRequest<ErrorOr<bool>>;