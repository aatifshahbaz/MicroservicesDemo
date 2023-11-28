using Catalog.Service.Data;
using Catalog.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Catalog.Service.Repositories
{
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DbSet<T> _table = null;
        private readonly ApplicationDbContext _context;

        public SqlRepository(ApplicationDbContext context)
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
            //It will mark the Entity state as Added State
            _table.Add(entity);

            return _context.SaveChanges() > 0;
        }


        public bool Update(T entity)
        {
            //First attach the object to the table
            _table.Attach(entity);
            //Then set the state of the Entity as Modified
            _context.Entry(entity).State = EntityState.Modified;

            return _context.SaveChanges() > 0;
        }
        public bool Delete(T entity)
        {
            //First, fetch the record from the table
            T existing = _table.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            //This will mark the Entity State as Deleted
            _table.Remove(existing);

            return _context.SaveChanges() > 0;
        }

    }
}
