using AutoMapper;
using DiscountGrpcService.Protos;
using DiscountGrpcService.Repositories.Interfaces;
using Grpc.Core;

namespace DiscountGrpcService.Services;

public class DiscountGrpcService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DiscountGrpcService> _logger;

    public DiscountGrpcService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountGrpcService> logger)
    {
        _discountRepository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task<DiscountResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var discount = await _discountRepository.GetDiscount(request.AdvertId);
        if (discount == null) return new DiscountResponse{ CalculatedPrice = request.Price};
        float calculatedPrice = request.Price - request.Price * discount.Percentage / 100;
        return new DiscountResponse
        {
            CalculatedPrice = calculatedPrice
        };
    }

    public override async Task<AddDiscountResponse> AddDiscount(AddDiscountRequest request, ServerCallContext context)
    {
        var model = _mapper.Map<Discount.Discount>(request);
        var discount = _discountRepository.AddDiscount(model);
        if(!discount.IsCompletedSuccessfully) throw new RpcException(new Status(StatusCode.NotFound, $"Not possible to create discount"));
        _logger.LogInformation("Discount was created. DiscountKey: {DiscountAdvertId}", request.Discount.AdvertId);
        return new AddDiscountResponse
        {
            Success = discount.IsCompletedSuccessfully
        };
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var discount = _discountRepository.DeleteDiscount(request.AdvertId);
        _logger.LogInformation("Discount with key {DiscountAdvertId} was deleted", request.AdvertId);
        return new DeleteDiscountResponse
        {
            Success = discount.IsCompletedSuccessfully
        };
    }
}