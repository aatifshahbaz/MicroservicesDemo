namespace Catalog.Service.Services
{
    public interface IItemService
    {
        public List<ItemDto> Get();

        public ItemDto GetById(Guid id);

        public ItemDto Create(CreateItemDto item);

        public bool Update(Guid id, UpdateItemDto item);

        public bool Delete(Guid id);
    }
}
