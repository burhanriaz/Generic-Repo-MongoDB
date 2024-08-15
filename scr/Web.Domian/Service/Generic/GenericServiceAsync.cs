using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;
using Web.Domain;
using Web.Domain.Infrastructure;
using Web.Domain.Service.Abstractions.Generic;
using Web.Domian.Infrastructure;
using Web.Entity;
using Web.Entity.UnitOfWork;

namespace Web.Domain.Service.Generic
{
    [SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1401:FieldsMustBePrivate",
        Justification = "UoW field is used in child classes ")]

    public class GenericServiceAsync<TViewModel, TEntity> : IServiceAsync<TViewModel, TEntity>
        where TViewModel : class, IBaseDomain
        where TEntity : class, IBaseEntity
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper Mapper;

        public GenericServiceAsync(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            Mapper = mapper;
        }

        public virtual async Task<IEnumerable<TViewModel>> GetAll()
        {
            var entities = await _unitOfWork.GetRepositoryAsync<TEntity>().GetAll();

            return Mapper.Map<IEnumerable<TViewModel>>(entities);
        }

        public virtual async Task<TViewModel> GetOne(Guid id)
        {
            var entity = await _unitOfWork.GetRepositoryAsync<TEntity>().GetOne(x => x.Id == id);

            return Mapper.Map<TViewModel>(entity);
        }

        public virtual async Task<OperationResult> Insert(TViewModel viewModel)
        {
            var entity = Mapper.Map<TEntity>(viewModel);
            var insertedEntity = await _unitOfWork.GetRepositoryAsync<TEntity>().Insert(entity);
            var insertedViewModel = Mapper.Map<TViewModel>(insertedEntity);

            return new OperationResult(true, "Inserted to db", insertedViewModel);
        }

        public virtual async Task<OperationResult> Update(TViewModel view)
        {
            await CheckIfEntityExistsAsync(view.Id);

            var updatedEntity = await _unitOfWork.GetRepositoryAsync<TEntity>().Update(view.Id, Mapper.Map<TEntity>(view));
            var updatedModel = Mapper.Map<TViewModel>(updatedEntity);

            return new OperationResult(true, null, updatedModel);
        }

        public virtual async Task<OperationResult> Delete(Guid id)
        {
            await CheckIfEntityExistsAsync(id);

            var numDeleted = await _unitOfWork.GetRepositoryAsync<TEntity>().Delete(id);
            if (numDeleted > 0)
            {
                return new OperationResult(true, $"Deleted count of {nameof(TEntity)}: {numDeleted}", numDeleted);
            }

            return new OperationResult(false, $"Not deleted {nameof(TEntity)} with id {id}");
        }

        public virtual async Task<IEnumerable<TViewModel>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            var items = await _unitOfWork.GetRepositoryAsync<TEntity>().Get(predicate);

            return Mapper.Map<IEnumerable<TViewModel>>(items);
        }

        private protected async Task<TEntity> CheckIfEntityExistsAsync(Guid id)
        {
            var entityToFind = await _unitOfWork.GetRepositoryAsync<TEntity>().GetOne(e => e.Id == id);
            if (entityToFind == null)
            {
                throw new ValidationException($"Entity not found: {nameof(TEntity)}, id: {id}");
            }

            return entityToFind;
        }
    }
}