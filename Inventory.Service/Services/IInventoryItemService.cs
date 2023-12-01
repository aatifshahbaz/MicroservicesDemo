namespace Inventory.Service.Services
{
    public interface IInventoryItemService
    {
        List<InventoryItemDto> Get(Guid userId);

        bool Create(GrantItemDto dto);
    }
}
