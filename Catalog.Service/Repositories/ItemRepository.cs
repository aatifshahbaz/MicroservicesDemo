using Catalog.Service.Data;
using Catalog.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Service.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _context;
        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<Item> Get()
        {
            return _context.Items.ToList();
        }

        public Item GetById(Guid id)
        {
            return _context.Items.AsNoTracking().FirstOrDefault(item => id == item.Id);
        }

        public bool Create(Item item)
        {
            _context.Items.Add(item);
            return _context.SaveChanges() > 0;
        }


        public bool Update(Item item)
        {
            _context.Items.Update(item);
            return _context.SaveChanges() > 0;
        }
        public bool Delete(Item item)
        {
            _context.Items.Remove(item);
            return _context.SaveChanges() > 0;
        }

    }
}
