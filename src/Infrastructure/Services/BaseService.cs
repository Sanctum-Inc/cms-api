using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Domain.Common;
using ErrorOr;
using MapsterMapper;
using MediatR;

namespace Infrastructure.Services.Base
{
    public abstract class BaseService<TEntity, TResult, TAddCommand, TUpdateCommand> : IBaseService<TResult>
        where TEntity : AuditableEntity
        where TResult : class
        where TAddCommand : IRequest<ErrorOr<Guid>>
        where TUpdateCommand : IRequest<ErrorOr<bool>>
    {
        private readonly IBaseRepository<TEntity> _repository;
        private readonly IMapper _mapper;
        private readonly ISessionResolver _sessionResolver;

        protected BaseService(IBaseRepository<TEntity> repository, IMapper mapper, ISessionResolver sessionResolver)
        {
            _repository = repository;
            _mapper = mapper;
            _sessionResolver = sessionResolver;
        }

        /// <summary>
        /// Creates a new entity from an AddCommand
        /// </summary>
        protected abstract ErrorOr<TEntity> MapFromAddCommand(TAddCommand command, string? userId = null);

        /// <summary>
        /// Applies updates from UpdateCommand to an existing entity
        /// </summary>
        protected abstract void MapFromUpdateCommand(TEntity entity, TUpdateCommand command);

        /// <summary>
        /// Optional override for resolving user context
        /// </summary>
        protected virtual string? ResolveUserId() => _sessionResolver.UserId;

        public virtual async Task<ErrorOr<Guid>> Add(IRequest<ErrorOr<Guid>> request, CancellationToken cancellationToken)
        {
            if (request is not TAddCommand addCommand)
                return Error.Failure(description: "Invalid request type.");

            ErrorOr<TEntity> entity = MapFromAddCommand(addCommand, ResolveUserId());
            if (entity.IsError) return entity.Errors;

            await _repository.AddAsync(entity.Value!, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return entity.Value.Id;
        }

        public virtual async Task<ErrorOr<bool>> Update(IRequest<ErrorOr<bool>> request, CancellationToken cancellationToken)
        {
            if (request is not TUpdateCommand updateCommand)
                return Error.Failure(description: "Invalid request type.");

            var entity = await _repository.GetByIdAndUserIdAsync(GetIdFromUpdateCommand(updateCommand), cancellationToken);
            if (entity == null)
                return Error.NotFound("Entity.NotFound", "Entity with given Id was not found.");

            MapFromUpdateCommand(entity, updateCommand);
            await _repository.UpdateAsync(entity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return true;
        }

        public virtual async Task<ErrorOr<bool>> Delete(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAndUserIdAsync(id, cancellationToken);
            if (entity == null)
                return Error.NotFound("Entity.NotFound", "Entity with given Id was not found.");

            await _repository.DeleteAsync(entity, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return true;
        }

        public virtual async Task<ErrorOr<IEnumerable<TResult>>> Get(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAll(cancellationToken);
            var results = entities.Select(_mapper.Map<TResult>);
            return results.ToErrorOr();
        }

        public virtual async Task<ErrorOr<TResult>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAndUserIdAsync(id, cancellationToken);
            return entity == null ?
                Error.NotFound("Entity.NotFound", "Entity with given Id was not found.") :
                _mapper.Map<TResult>(entity);
        }

        /// <summary>
        /// Extracts the entity ID from the UpdateCommand.
        /// </summary>
        protected abstract Guid GetIdFromUpdateCommand(TUpdateCommand command);
    }
}
