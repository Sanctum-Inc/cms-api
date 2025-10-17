using Application.Common.Models;
using Application.Lawyer.Commands.Add;
using Application.Lawyer.Queries.GetById;
using Domain.Lawyers;
using ErrorOr;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Common.Interfaces.Services;
public interface ILawyerService
{
    // Create
    Task<ErrorOr<bool>> Add(AddCommand lawyer, CancellationToken cancellationToken);

    // Read
    Task<ErrorOr<GetLawyerResult>> GetById(Guid Id, CancellationToken cancellationToken);
    Task<ErrorOr<List<GetLawyerResult>>> Get(CancellationToken cancellationToken);

    // Update
    Task<ErrorOr<bool>> Update(
        Guid id,
        string name,
        string surname,
        Speciality speciality,
        string mobileNumber,
        string emailAddress,
        CancellationToken cancellationToken);

    // Delete
    Task<ErrorOr<bool>> Delete(Guid lawyerId, CancellationToken cancellationToken);
}
