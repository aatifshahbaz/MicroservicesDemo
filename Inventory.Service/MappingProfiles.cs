using AutoMapper;
using Inventory.Service.Models;

namespace Inventory.Service
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<InventoryItem, InventoryItemDto>().ReverseMap();
            CreateMap<GrantItemDto, InventoryItem>().ReverseMap();
        }
    }
}
