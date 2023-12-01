using Common.Entity;
using System.Linq.Expressions;

namespace Common.Repository
{
    public interface IRepository<T> where T : IEntity
    {
        bool Create(T entity);
        bool Delete(T entity);
        List<T> Get();
        List<T> Get(Expression<Func<T, bool>> filter);
        T GetBy(Guid id);
        T GetBy(Expression<Func<T, bool>> filter);
        bool Update(T entity);
    }
}