using AutoMapper;
using Catalog.Service.Models;

namespace Catalog.Service
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<CreateItemDto, Item>().ReverseMap();
            CreateMap<UpdateItemDto, Item>().ReverseMap();

        }
    }
}
