using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Web.Entity.Enum;

namespace Web.Entity.Repository.Abstractions.Generic
{
    public interface IRepositoryAsync<T>  where T : class, IBaseEntity
    {
        Task<IEnumerable<T>> GetAll();

        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetSkipTake(Expression<Func<T, bool>> predicate, int skip, int take);
        Task<int> GetCount(Expression<Func<T, bool>> predicate);

        Task<T> GetOne(Expression<Func<T, bool>> predicate);

        Task<T> Insert(T entity);

        Task<long> Delete(T entity);

        Task<long> Delete(Guid id);

        Task<T> Update(Guid id, T entity, bool updateCreateTime = false, bool updateUpdatedTime = true);

        Task<long> Count(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetSkipTakeWithSort(Expression<Func<T, bool>> predicate, int skip, int take, OrderByEnum orderByEnum);
    }
}
