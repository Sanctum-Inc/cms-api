using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.InvoiceItem.Commands.Add;
using Application.InvoiceItem.Commands.Update;
using Domain.CourtCases;
using Domain.InvoiceItems;
using Domain.Users;
using ErrorOr;
using Infrastructure.Services.Base;
using MapsterMapper;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Services;
public class InvoiceItemService : BaseService<InvoiceItem, InvoiceItemResult, AddCommand, UpdateCommand>, IInvoiceItemService
{
    private readonly ISessionResolver _sessionResolver;
    private readonly IInvoiceItemRepository _repository;
    private readonly ICourtCaseService _courtCaseService;

    public InvoiceItemService(
        IInvoiceItemRepository repository,
        IMapper mapper,
        ISessionResolver sessionResolver,
        ICourtCaseService courtCaseService) : base(repository, mapper, sessionResolver)
    {
        _sessionResolver = sessionResolver;
        _repository = repository;
        _courtCaseService = courtCaseService;
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }

    protected override ErrorOr<InvoiceItem> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        return new InvoiceItem()
        {
            InvoiceId = new Guid(command.InvoiceId),
            Name = command.Name,
            Hours = command.Hours,
            CostPerHour = command.CostPerHour,
            Id = Guid.NewGuid(),
            UserId = new Guid(userId),
        };
    }

    protected override void MapFromUpdateCommand(InvoiceItem entity, UpdateCommand command)
    {
        entity.InvoiceId = command.InvoiceId;
        entity.Name = command.Name;
        entity.Hours = command.Hours;
        entity.CostPerHour = command.CostPerHour;
    }

    //public async override Task<ErrorOr<Guid>> Add(IRequest<ErrorOr<Guid>> request, CancellationToken cancellationToken)
    //{
    //    if (request is not AddCommand addCommand)
    //        return Error.Failure(description: "Invalid request type.");

    //    var courtCase = await _courtCaseService.GetById(addCommand.CaseId, cancellationToken);
    //    if (courtCase.IsError)
    //    {
    //        return courtCase.Errors;
    //    }

    //    if (addCommand.InvoiceId.ToLower() == "new")
    //    {
    //        var invoiceCommand = new Application.Invoice.Commands.Add.AddCommand(
    //            Guid.NewGuid().ToString(),
    //            DateTime.Now,
    //            courtCase.Value.Lawyers,
    //            );
    //        var entity = new InvoiceItem()
    //        {
    //            InvoiceId = new Guid(invoiceCommand.InvoiceNumber),
    //            Name = addCommand.Name,
    //            Hours = addCommand.Hours,
    //            CostPerHour = addCommand.CostPerHour,
    //            Id = Guid.NewGuid(),
    //            UserId = new Guid(_sessionResolver.UserId!),
    //        };

    //        await _repository.AddAsync(entity, cancellationToken);
    //        await _repository.SaveChangesAsync(cancellationToken);

    //        return entity.Id;
    //    }
    //    else
    //    {
    //        return await base.Add(request, cancellationToken);
    //    }
    //}
}
