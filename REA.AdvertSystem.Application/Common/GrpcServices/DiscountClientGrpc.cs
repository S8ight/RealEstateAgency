
using DiscountGrpcService.Protos;

namespace REA.AdvertSystem.Application.Common.GrpcServices;

public class DiscountClientGrpc
{
    private readonly DiscountProtoService.DiscountProtoServiceClient
        _discountProtoClient;

    public DiscountClientGrpc(DiscountProtoService.DiscountProtoServiceClient discountProtoClient)
    {
        _discountProtoClient = discountProtoClient;
    }

    public async Task<DiscountResponse> GetDiscount(string advertId, float price)
    {
        var request = new GetDiscountRequest { AdvertId = advertId, Price = price};
        
        return await _discountProtoClient.GetDiscountAsync(request);
    }

    public async Task DeleteAdvertDiscounts(string advertId)
    {
        var request = new DeleteDiscountRequest { AdvertId = advertId };
        
        await _discountProtoClient.DeleteAdvertDiscountsAsync(request);
    }
}