using System.ComponentModel.DataAnnotations;

namespace Inventory.Service
{
    //Records are simpler, immutable, value based equality and have builtin ToString override
    public record InventoryItemDto(Guid CatalogItemId, int Quantity, DateTimeOffset AcquiredDate);

    public record GrantItemDto([Required] Guid UserId, [Required] Guid CatalogItemId, [Range(0, 100)] int Quantity);
}