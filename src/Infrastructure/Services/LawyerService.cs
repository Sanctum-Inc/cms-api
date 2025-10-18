using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Lawyer.Commands.Add;
using Domain.Lawyers;
using ErrorOr;
using MapsterMapper;

namespace Infrastructure.Services;
internal class LawyerService : ILawyerService
{
    private readonly ILawyerRepository _lawyerRepository;
    private readonly ISessionResolver _sessionResolver;
    private readonly IMapper _mapper;

    public LawyerService(
        ILawyerRepository lawyerRepository,
        ISessionResolver sessionResolver,
        IMapper mapper)
    {
        _lawyerRepository = lawyerRepository;
        _sessionResolver = sessionResolver;
        _mapper = mapper;
    }

    public async Task<ErrorOr<bool>> Add(AddCommand lawyer, CancellationToken cancellationToken)
    {
        var userId = _sessionResolver.UserId;
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        await _lawyerRepository.AddAsync(new Lawyer
        {
            Id = Guid.NewGuid(),
            Name = lawyer.Name,
            Surname = lawyer.Surname,
            Specialty = lawyer.Specialty,
            MobileNumber = lawyer.MobileNumber,
            Email = lawyer.Email,
            UserId = new Guid(userId),
        }, cancellationToken);
        await _lawyerRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<bool>> Delete(Guid Id, CancellationToken cancellationToken)
    {
        var lawyer = await _lawyerRepository.GetByIdAsync(Id, cancellationToken);

        if (lawyer == null)
            return Error.NotFound("Lawyer.NotFound", "Lawyer with given Id was not found.");


        await _lawyerRepository.DeleteAsync(lawyer, cancellationToken);
        await _lawyerRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<List<GetLawyerResult>>> Get(CancellationToken cancellationToken)
    {
        var result = await _lawyerRepository.GetAll(cancellationToken);

        return _mapper.Map<List<GetLawyerResult>>(result);
    }

    public async Task<ErrorOr<GetLawyerResult>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _lawyerRepository.GetByIdAsync(id, cancellationToken);

        return _mapper.Map<GetLawyerResult>(result!);
    }

    public async Task<ErrorOr<bool>> Update(
        Guid id,
        string name,
        string surname,
        Speciality speciality,
        string mobileNumber,
        string emailAddress,
        CancellationToken cancellationToken)
    {

        var userId = _sessionResolver.UserId;
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        var lawyer = await _lawyerRepository.GetByIdAsync(id, cancellationToken);

        if (lawyer == null)
            return Error.NotFound("Lawyer.NotFound", "Lawyer with given Id was not found.");

        lawyer.Name = name;
        lawyer.Surname = surname;
        lawyer.Specialty = speciality;
        lawyer.MobileNumber = mobileNumber;
        lawyer.Email = emailAddress;

        await _lawyerRepository.UpdateAsync(lawyer, cancellationToken);
        await _lawyerRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
