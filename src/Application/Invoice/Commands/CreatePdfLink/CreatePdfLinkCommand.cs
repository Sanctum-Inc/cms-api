using Application.Common.Models;
using Application.Document.Queries.Get;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.CreatePdfLink;

public record CreatePdfLinkCommand(
    Guid Id) : IRequest<ErrorOr<string>>;
