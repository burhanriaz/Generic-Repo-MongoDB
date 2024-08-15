using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Web.Entity.Infrastructure.Options;
using Web.Entity.Repository.Abstractions.Generic;
using Web.Entity.Repository.Generic;
using AspNetCore.Identity.MongoDbCore;
using Web.Entity.Entity.Identity;
using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Entity.UnitOfWork
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private readonly IMongoDatabase _database;
        private readonly Dictionary<string, string> _collectionNames;

        private ConcurrentDictionary<Type, object> _repositoriesAsync = new ConcurrentDictionary<Type, object>();

        public MongoUnitOfWork(IOptions<MongoSecretsSettings> mongoSecrets, IOptions<MongoSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSecrets.Value.MongoConnectionString);
            _database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _collectionNames = mongoSettings.Value.CollectionNames;
        }

        public GenericRepositoryAsync<TEntity> GetGenericRepositoryAsync<TEntity>() where TEntity : class, IBaseEntity
        {
            return (GenericRepositoryAsync<TEntity>)_repositoriesAsync.GetOrAdd(typeof(TEntity), (collectionType) =>
            {
                var name = _collectionNames[collectionType.Name];
                var collection = _database.GetCollection<TEntity>(name);
                return new GenericRepositoryAsync<TEntity>(collection);
            });
        }

        public IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class, IBaseEntity
        {
            return (IRepositoryAsync<TEntity>)_repositoriesAsync.GetOrAdd(typeof(TEntity), (collectionType) =>
            {
                var name = _collectionNames[collectionType.Name];
                var collection = _database.GetCollection<TEntity>(name);
                return new GenericRepositoryAsync<TEntity>(collection);
            });
        }

        //public GenericRepositoryAsync<TEntity> GetGenericRepositoryAsync<TEntity>()
        //{
        //    return (GenericRepositoryAsync<TEntity>)_repositoriesAsync.GetOrAdd(typeof(TEntity), (collectionType) =>
        //    {
        //        var name = _collectionNames[collectionType.Name];
        //        var collection = _database.GetCollection<TEntity>(name);
        //        return new GenericRepositoryAsync<TEntity>(collection);
        //    });
        //}

        //public IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : BaseEntity
        //{
        //    return (IRepositoryAsync<TEntity>)_repositoriesAsync.GetOrAdd(typeof(TEntity), (collectionType) =>
        //    {
        //        var name = _collectionNames[collectionType.Name];
        //        var collection = _database.GetCollection<TEntity>(name);
        //        return new GenericRepositoryAsync<TEntity>(collection);
        //    });
        //}

        //GenericRepositoryAsync<TEntity> IUnitOfWork.GetGenericRepository<TEntity>()
        //{
        //    return (GenericRepositoryAsync<TEntity>)_repositoriesAsync.GetOrAdd(typeof(TEntity), (collectionType) =>
        //    {
        //        var name = _collectionNames[collectionType.Name];
        //        var collection = _database.GetCollection<TEntity>(name);
        //        return new GenericRepositoryAsync<TEntity>(collection);
        //    });
        //}

        //IRepositoryAsync<TEntity> IUnitOfWork.GetRepository<TEntity>()
        //{
        //    return (GenericRepositoryAsync<TEntity>)_repositoriesAsync.GetOrAdd(typeof(TEntity), (collectionType) =>
        //    {
        //        var name = _collectionNames[collectionType.Name];
        //        var collection = _database.GetCollection<TEntity>(name);
        //        return new GenericRepositoryAsync<TEntity>(collection);
        //    });
        //}
    }

}