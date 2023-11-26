using Catalog.Service.Models;

namespace Catalog.Service.Repositories
{
    public interface IItemRepository
    {
        public List<Item> Get();

        public Item GetById(Guid id);

        public bool Create(Item item);

        public bool Update(Item item);

        public bool Delete(Item item);

    }
}
