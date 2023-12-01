using Common.Data;
using Common.Entity;
using Microsoft.EntityFrameworkCore;

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

        public T GetById(Guid id)
        {
            return _table.AsNoTracking().FirstOrDefault(e => e.Id == id);
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
