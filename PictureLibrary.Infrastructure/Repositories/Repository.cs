using MongoDB.Bson;
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

        public TEntity? FindById(ObjectId id)
        {
            return Query().FirstOrDefault(x => x.Id == id);
        }

        public async Task Add(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task Update(TEntity entity)
        {
            await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
        }

        public async Task Delete(TEntity entity)
        {
            await _collection.DeleteOneAsync(e => e.Id == entity.Id);
        }

        public IQueryable<TEntity> Query()
        {
            return _collection.AsQueryable();
        }
    }
}
