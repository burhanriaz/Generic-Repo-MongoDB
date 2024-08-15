using AspNetCore.Identity.MongoDbCore;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using Web.Entity.Entity.Identity;
using Web.Entity.Repository.Abstractions.Generic;
using Web.Entity.Repository.Generic;

namespace Web.Entity.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class, IBaseEntity;

        GenericRepositoryAsync<TEntity> GetGenericRepositoryAsync<TEntity>() where TEntity : class, IBaseEntity;

        //IRepositoryAsync<TEntity> GetRepository<TEntity>() where TEntity : class, IBaseEntity;
        //GenericRepositoryAsync<TEntity> GetGenericRepository<TEntity>() where TEntity : class, IBaseEntity;      
    }
}
