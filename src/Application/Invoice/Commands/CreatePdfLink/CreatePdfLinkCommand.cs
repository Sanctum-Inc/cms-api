using ErrorOr;
using MediatR;

namespace Application.Invoice.Commands.CreatePdfLink;

public record CreatePdfLinkCommand(
    Guid Id) : IRequest<ErrorOr<string>>;
