using AutoMapper;
using Common.Repository;
using Inventory.Service.Models;
using System.Linq.Expressions;

namespace Inventory.Service.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IRepository<InventoryItem> _invItemRepository;
        private readonly IRepository<CatalogItem> _catalogItemRepository;
        private readonly IMapper _mapper;

        public InventoryItemService(IMapper mapper, IRepository<InventoryItem> invItemRepository, IRepository<CatalogItem> catalogItemRepository)
        {
            _mapper = mapper;
            _invItemRepository = invItemRepository;
            _catalogItemRepository = catalogItemRepository;
        }

        public List<InventoryItemDto> Get(Guid userId)
        {
            var catalogItems = _catalogItemRepository.Get();
            var inventoryItems = _invItemRepository.Get(item => item.UserId == userId);

            if (inventoryItems == null)
                return null;

            var inventoryItemDto = inventoryItems.Select(inventoryItem =>
            {
                var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);

                return _mapper.Map<InventoryItem, InventoryItemDto>(inventoryItem, options =>
                {
                    options.AfterMap((src, dest) =>
                    {
                        dest.Name = catalogItem.Name;
                        dest.Description = catalogItem.Description;
                    });
                });
            });

            return inventoryItemDto.ToList();
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
