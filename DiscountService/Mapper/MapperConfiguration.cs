using AutoMapper;
using DiscountService.DTO;
using DiscountService.Entities;

namespace DiscountService.Mapper;

public class MapperConfiguration : Profile
{
    public MapperConfiguration()
    {
        CreateMap<DiscountRequest, Discount>();
    }
}