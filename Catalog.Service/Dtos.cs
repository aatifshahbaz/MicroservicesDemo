using System.ComponentModel.DataAnnotations;

namespace Catalog.Service
{
    //Records are simpler, immutable, value based equality and have builtin ToString override
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);

    public record CreateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

    public record UpdateItemDto([Required] string Name, string Description, [Range(0, 1000)] decimal Price);

}