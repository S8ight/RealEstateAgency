using DiscountGrpcService.Protos;

namespace REA.AdvertSystem.Application.Common.GrpcServices;

public class DiscountServiceGrpc
{
    private readonly DiscountProtoService.DiscountProtoServiceClient
        _discountProtoService;

    public DiscountServiceGrpc(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    {
        _discountProtoService = discountProtoService;
    }

    public async Task<DiscountResponse> GetDiscount(string advertId, float price)
    {
        var request = new GetDiscountRequest { AdvertId = advertId, Price = price};
        return await _discountProtoService.GetDiscountAsync(request);
    }
}