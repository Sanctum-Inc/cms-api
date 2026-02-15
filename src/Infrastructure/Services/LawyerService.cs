using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Lawyer.Commands.Add;
using Application.Lawyer.Commands.Update;
using Domain.Lawyers;
using ErrorOr;
using MapsterMapper;

namespace Infrastructure.Services;

public class LawyerService : BaseService<Lawyer, LawyerResult, AddCommand, UpdateCommand>, ILawyerService
{
    private readonly ILawyerRepository _lawyerRepository;

    public LawyerService(
        ILawyerRepository lawyerLawyerRepository,
        ISessionResolver sessionResolver,
        IMapper mapper) : base(lawyerLawyerRepository, mapper, sessionResolver)
    {
        _lawyerRepository = lawyerLawyerRepository;
    }

    protected override ErrorOr<Lawyer> MapFromAddCommand(AddCommand command, string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Error.Unauthorized(description: "User is not authenticated.");
        }

        return new Lawyer
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Surname = command.Surname,
            Specialty = command.Specialty,
            MobileNumber = command.MobileNumber,
            Email = command.Email,
            UserId = Guid.Parse(userId)
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

    public async override Task<ErrorOr<IEnumerable<LawyerResult>>> Get(CancellationToken cancellationToken)
    {
        var result = await _lawyerRepository.GetAll(cancellationToken);

        return result
            .Select(x => new LawyerResult()
            {
                Email = x.Email,
                MobileNumber = x.MobileNumber,
                Name = x.Name,
                Speciality = x.Specialty,
                Surname = x.Surname,
                Id = x.Id,
                TotalCases = x.CourtCases.Count
            })
            .ToList();
    }
}
