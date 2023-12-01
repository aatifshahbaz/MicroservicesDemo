using Common.Data;
using Common.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Repository
{
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DbSet<T> _table = null;
        private readonly IDbContext _context;

        public SqlRepository(IDbContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }


        public List<T> Get()
        {
            return _table.ToList();
        }

        public List<T> Get(Expression<Func<T, bool>> filter)
        {
            return _table.Where(filter).ToList();
        }

        public T GetBy(Guid id)
        {
            return _table.AsNoTracking().FirstOrDefault(e => e.Id == id);
        }

        public T GetBy(Expression<Func<T, bool>> filter)
        {
            return _table.AsNoTracking().FirstOrDefault(filter);
        }

        public bool Create(T entity)
        {
            _table.Add(entity);
            return _context.SaveChanges() > 0;
        }


        public bool Update(T entity)
        {
            _table.Update(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(T entity)
        {
            _table.Remove(entity);
            return _context.SaveChanges() > 0;
        }
    }
}
