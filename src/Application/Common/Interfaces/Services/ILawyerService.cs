using Application.Common.Models;
using Application.Lawyer.Commands.Add;
using Domain.Lawyers;
using ErrorOr;

namespace Application.Common.Interfaces.Services;

/// <summary>
/// Provides a set of operations for managing lawyer records.
/// </summary>
public interface ILawyerService : IBaseService<LawyerResult>
{
}
