using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Firm.Commands.Add;
using Application.Firm.Commands.Update;
using Domain.Firms;
using ErrorOr;
using MapsterMapper;

namespace Infrastructure.Services;

public class FirmService : BaseService<Firm, FirmResult, AddCommand, UpdateCommand>, IFirmService
{
    public readonly IFirmRepository _firmRepository;

    public FirmService(
        IMapper mapper,
        IFirmRepository firmRepository,
        ISessionResolver sessionResolver) : base(firmRepository, mapper, sessionResolver)
    {
        _firmRepository = firmRepository;
    }

    public override async Task<ErrorOr<FirmResult>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _firmRepository.GetByIdAsync(id, cancellationToken);

        if (result is null)
        {
            return Error.NotFound("Firm.NotFound", "Firm with given Id was not found.");
        }

        return new FirmResult
        {
            Id = result.Id,
            AccountName = result.AccountName,
            Address = result.Address,
            AccountNumber = result.AccountNumber,
            AdvocateAdmissionDate = result.AdvocateAdmissionDate.ToString(),
            AttorneyAdmissionDate = result.AttorneyAdmissionDate.ToString(),
            Bank = result.Bank,
            BranchCode = result.BranchCode,
            Email = result.Email,
            Fax = result.Fax,
            Mobile = result.Mobile,
            Name = result.Name,
            Telephone = result.Telephone
        };
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }


    protected override ErrorOr<Firm> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        var firm = new Firm
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Address = command.Address,
            Telephone = command.Telephone,
            Fax = command.Fax,
            Mobile = command.Mobile,
            Email = command.Email,
            AttorneyAdmissionDate = DateTime.Parse(command.AttorneyAdmissionDate),
            AdvocateAdmissionDate = DateTime.Parse(command.AdvocateAdmissionDate),
            AccountName = command.AccountName,
            Bank = command.Bank,
            BranchCode = command.BranchCode,
            AccountNumber = command.AccountNumber
        };

        return firm;
    }

    protected override void MapFromUpdateCommand(Firm entity, UpdateCommand command)
    {
        entity.Name = command.Name;
        entity.Address = command.Address;
        entity.Telephone = command.Telephone;
        entity.Fax = command.Fax;
        entity.Mobile = command.Mobile;
        entity.Email = command.Email;
        entity.AttorneyAdmissionDate = DateTime.Parse(command.AttorneyAdmissionDate);
        entity.AdvocateAdmissionDate = DateTime.Parse(command.AdvocateAdmissionDate);
        entity.AccountName = command.AccountName;
        entity.Bank = command.Bank;
        entity.BranchCode = command.BranchCode;
        entity.AccountNumber = command.AccountNumber;
    }
}
