using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : IEntity
    {
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
        IQueryable<TEntity> Query();
    }
}
