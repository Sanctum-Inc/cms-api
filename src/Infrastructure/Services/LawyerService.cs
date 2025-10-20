using System.Net.Mail;
using System.Xml.Linq;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Common.Models;
using Application.Lawyer.Commands.Add;
using Application.Lawyer.Commands.Update;
using Domain.Lawyers;
using ErrorOr;
using MapsterMapper;
using MediatR;

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

    public async Task<ErrorOr<bool>> Add(IRequest<ErrorOr<bool>> request, CancellationToken cancellationToken)
    {
        if (request is not AddCommand addCommand)
            return Error.Failure(description: "Invalid request type.");

        var userId = _sessionResolver.UserId;
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        await _lawyerRepository.AddAsync(new Lawyer
        {
            Id = Guid.NewGuid(),
            Name = addCommand.Name,
            Surname = addCommand.Surname,
            Specialty = addCommand.Specialty,
            MobileNumber = addCommand.MobileNumber,
            Email = addCommand.Email,
            UserId = new Guid(userId),
        }, cancellationToken);
        await _lawyerRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<bool>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var lawyer = await _lawyerRepository.GetByIdAsync(id, cancellationToken);

        if (lawyer == null)
            return Error.NotFound("Lawyer.NotFound", "Lawyer with given Id was not found.");


        await _lawyerRepository.DeleteAsync(lawyer, cancellationToken);
        await _lawyerRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<LawyerResult?>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _lawyerRepository.GetByIdAsync(id, cancellationToken);
        if (result == null)
            return Error.NotFound("Lawyer.NotFound", "Lawyer with given Id was not found.");

        return _mapper.Map<LawyerResult>(result!);
    }


    public async Task<ErrorOr<bool>> Update(IRequest<ErrorOr<bool>> request, CancellationToken cancellationToken)
    {
        if (request is not UpdateCommand updateCommand)
            return Error.Failure(description: "Invalid request type.");

        var userId = _sessionResolver.UserId;
        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized(description: "User is not authenticated.");

        var lawyer = await _lawyerRepository.GetByIdAsync(updateCommand.Id, cancellationToken);

        if (lawyer == null)
            return Error.NotFound("Lawyer.NotFound", "Lawyer with given Id was not found.");

        lawyer.Name = updateCommand.Name;
        lawyer.Surname = updateCommand.Surname;
        lawyer.Specialty = updateCommand.Specialty;
        lawyer.MobileNumber = updateCommand.MobileNumber;
        lawyer.Email = updateCommand.Email;

        await _lawyerRepository.UpdateAsync(lawyer, cancellationToken);
        await _lawyerRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<IEnumerable<LawyerResult>>> Get(CancellationToken cancellationToken)
    {
        var result = await _lawyerRepository.GetAll(cancellationToken);

        return _mapper.Map<List<LawyerResult>>(result);
    }
}
