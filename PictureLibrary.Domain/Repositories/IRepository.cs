using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : IEntity, new()
    {
    }
}
