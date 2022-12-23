using AutoMapper;
using DiscountGrpcService.Protos;

namespace DiscountGrpcService.Extensions;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<Discount.Discount, AddDiscountRequest>();
        CreateMap<AddDiscountRequest, Discount.Discount>();
    }
}