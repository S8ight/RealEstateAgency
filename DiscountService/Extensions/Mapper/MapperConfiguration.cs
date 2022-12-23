using AutoMapper;
using DiscountService.DTO;
using DiscountService.Entities;

namespace DiscountService.Extensions.Mapper;

public class MapperConfiguration : Profile
{
    public MapperConfiguration()
    {
        CreateMap<Discount, DiscountRequest>();
        CreateMap<DiscountRequest, Discount>();
    }
}