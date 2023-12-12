using System.ComponentModel.DataAnnotations;

namespace Inventory.Service
{
    //Records are simpler, immutable, value based equality and have builtin ToString override
    //Had to convert to class as Automapper AfterMap not working with Record type
    public class InventoryItemDto
    {
        public Guid CatalogItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }
    }

    public record GrantItemDto([Required] Guid UserId, [Required] Guid CatalogItemId, [Range(0, 100)] int Quantity);
}