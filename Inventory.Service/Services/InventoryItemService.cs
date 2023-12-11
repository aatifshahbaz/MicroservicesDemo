using AutoMapper;
using Common.Repository;
using Inventory.Service.Models;
using System.Linq.Expressions;

namespace Inventory.Service.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IRepository<InventoryItem> _invItemRepository;
        private readonly IMapper _mapper;

        public InventoryItemService(IRepository<InventoryItem> invItemRepository, IMapper mapper)
        {
            _invItemRepository = invItemRepository;
            _mapper = mapper;
        }

        public List<InventoryItemDto> Get(Guid userId)
        {
            var items = _invItemRepository.Get(item => item.UserId == userId);

            return items.Count > 0 ? _mapper.Map<List<InventoryItemDto>>(items) : null;
        }


        public bool Create(GrantItemDto dto)
        {
            var inventoryItem = _invItemRepository
                .GetBy(i => i.UserId == dto.UserId && i.CatalogItemId == dto.CatalogItemId);

            if (inventoryItem == null)
            {
                var item = _mapper.Map<InventoryItem>(dto);
                item.Id = Guid.NewGuid();
                item.AcquiredDate = DateTime.UtcNow;

                return _invItemRepository.Create(item);
            }
            else
            {
                inventoryItem.Quantity += dto.Quantity;
                return _invItemRepository.Update(inventoryItem);
            }


        }
    }
}
