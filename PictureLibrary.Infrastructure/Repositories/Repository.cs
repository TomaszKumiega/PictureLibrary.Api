using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Infrastructure.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : IEntity
    {
        protected readonly IMongoCollection<TEntity> _collection;
        
        protected abstract string CollectionName { get; }

        public Repository(
            IAppSettings appSettings,
            IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(appSettings.DatabaseName);
            _collection = database.GetCollection<TEntity>(CollectionName);
        }

        public IQueryable<TEntity> Query()
        {
            return _collection.AsQueryable();
        }
    }
}
