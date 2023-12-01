using AutoMapper;
using Catalog.Service.Models;
using Common.Repository;

namespace Catalog.Service.Services
{
    public class ItemService : IItemService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IMapper _mapper;

        public ItemService(IRepository<Item> itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public List<ItemDto> Get()
        {
            var items = _itemRepository.Get();

            return items != null ? _mapper.Map<List<ItemDto>>(items) : null;
        }

        public ItemDto GetById(Guid id)
        {
            var item = _itemRepository.GetById(id);
            return item != null ? _mapper.Map<ItemDto>(item) : null;
        }
        public ItemDto Create(CreateItemDto dto)
        {
            var item = _mapper.Map<Item>(dto);
            item.Id = Guid.NewGuid();
            item.CreatedDate = DateTime.UtcNow;

            return _itemRepository.Create(item) ? _mapper.Map<ItemDto>(item) : null;
        }

        public bool Update(Guid id, UpdateItemDto dto)
        {
            var existingItem = _itemRepository.GetById(id);

            if (existingItem != null)
            {
                var item = _mapper.Map<Item>(dto);
                item.Id = existingItem.Id;
                item.CreatedDate = existingItem.CreatedDate;
                return _itemRepository.Update(item);
            }

            return false;
        }

        public bool Delete(Guid id)
        {
            var existingItem = _itemRepository.GetById(id);

            if (existingItem != null)
                return _itemRepository.Delete(existingItem);

            return false;
        }

    }
}
