using ErrorOr;
using MediatR;

namespace Application.Common.Interfaces.Services;

/// <summary>
///     Defines a set of base service operations for managing entities of type <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The entity type the service operates on.</typeparam>
public interface IBaseService<T>
    where T : class
{
    /// <summary>
    ///     Adds a new entity to the system.
    /// </summary>
    /// <param name="request">The MediatR request containing the entity's data to create.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>
    ///     An <see cref="ErrorOr{T}" /> result containing <c>true</c> if the entity was successfully created,
    ///     or an error if the operation failed.
    /// </returns>
    Task<ErrorOr<Guid>> Add(IRequest<ErrorOr<Guid>> request, CancellationToken cancellationToken);


    /// <summary>
    ///     Deletes an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>
    ///     An <see cref="ErrorOr{T}" /> result containing <c>true</c> if the entity was successfully deleted,
    ///     or an error if the operation failed.
    /// </returns>
    Task<ErrorOr<bool>> Delete(Guid id, CancellationToken cancellationToken);

    /// <summary>
    ///     Updates an existing entity in the system.
    /// </summary>
    /// <param name="request">The MediatR request containing the updated entity data.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>
    ///     An <see cref="ErrorOr{T}" /> result containing <c>true</c> if the update was successful,
    ///     or an error if the operation failed.
    /// </returns>
    Task<ErrorOr<bool>> Update(IRequest<ErrorOr<bool>> request, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves all entities of type <typeparamref name="T" />.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>
    ///     An <see cref="ErrorOr{T}" /> result containing a collection of entities,
    ///     or an error if the operation failed.
    /// </returns>
    Task<ErrorOr<IEnumerable<T>>> Get(CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves a single entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>
    ///     An <see cref="ErrorOr{T}" /> result containing the entity if found,
    ///     or an error if the entity does not exist or the operation failed.
    /// </returns>
    Task<ErrorOr<T>> GetById(Guid id, CancellationToken cancellationToken);
}
