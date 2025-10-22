using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Lawyer.Commands.Add;
using Application.Lawyer.Commands.Update;
using Domain.Lawyers;
using ErrorOr;
using Infrastructure.Services.Base;
using MapsterMapper;
using MediatR;

namespace Infrastructure.Services;
public class LawyerService : BaseService<Lawyer, LawyerResult, AddCommand, UpdateCommand>, ILawyerService
{

    public LawyerService(
        ILawyerRepository lawyerRepository,
        ISessionResolver sessionResolver,
        IMapper mapper): base(lawyerRepository, mapper, sessionResolver)
    {
    }

    protected override ErrorOr<Lawyer> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        return new Lawyer
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Surname = command.Surname,
            Specialty = command.Specialty,
            MobileNumber = command.MobileNumber,
            Email = command.Email,
            UserId = new Guid(userId),
        };
    }

    protected override void MapFromUpdateCommand(Lawyer entity, UpdateCommand command)
    {
        entity.Name = command.Name;
        entity.Surname = command.Surname;
        entity.Specialty = command.Specialty;
        entity.MobileNumber = command.MobileNumber;
        entity.Email = command.Email;
    }

    protected override Guid GetIdFromUpdateCommand(UpdateCommand command)
    {
        return command.Id;
    }
}
