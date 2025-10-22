using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.InvoiceItem.Commands.Add;
using Application.InvoiceItem.Commands.Update;
using Domain.CourtCases;
using Domain.InvoiceItems;
using ErrorOr;
using Infrastructure.Services.Base;
using MapsterMapper;

namespace Infrastructure.Services;
public class InvoiceItemService : BaseService<InvoiceItem, InvoiceItemResult, AddCommand, UpdateCommand>, IInvoiceItemService
{

    public InvoiceItemService(IInvoiceItemRepository repository, IMapper mapper, ISessionResolver sessionResolver) : base(repository, mapper, sessionResolver)
    {
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
            InvoiceId = command.InvoiceId,
            Name = command.Name,
            Hours = command.Hours,
            CostPerHour = command.CostPerHour,
            DayFeeAmount = command.DayFeeAmount,
            IsDayFee = command.IsDayFee,
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
        entity.DayFeeAmount = command.DayFeeAmount;
        entity.IsDayFee = command.IsDayFee;
    }
}
