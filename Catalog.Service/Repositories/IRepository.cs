using Catalog.Service.Models;

namespace Catalog.Service.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        bool Create(T entity);
        bool Delete(T entity);
        List<T> Get();
        T GetById(Guid id);
        bool Update(T entity);
    }
}