using System.Linq.Expressions;
using Web.Domain.Infrastructure;

namespace Web.Domain.Service.Abstractions.Generic
{
    public interface IServiceAsync<TViewModel, TEntity>
    {
        Task<IEnumerable<TViewModel>> GetAll();

        Task<OperationResult> Insert(TViewModel accountVm);

        Task<OperationResult> Update(TViewModel obj);

        Task<OperationResult> Delete(Guid id);

        Task<TViewModel> GetOne(Guid id);

        Task<IEnumerable<TViewModel>> Get(Expression<Func<TEntity, bool>> predicate);
    }
}