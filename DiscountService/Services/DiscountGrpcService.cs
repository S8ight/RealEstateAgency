using AutoMapper;
using DiscountGrpcService.Protos;
using DiscountService.DataAccess.Interfaces;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

namespace DiscountService.Services;

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
         var discount = await _discountRepository.GetAdvertCurrentDiscount(request.AdvertId);
         if (discount == null)
         {
             return new DiscountResponse();
         }
         
         var calculatedPrice = request.Price - request.Price * discount.Percentage / 100;
         
         return new DiscountResponse
         {
             CalculatedPrice = calculatedPrice,
             ExpiresAt = Timestamp.FromDateTime(discount.ExpireAt)
         };
     }

     public override async Task<Empty> DeleteAdvertDiscounts(DeleteDiscountRequest request, ServerCallContext context)
     {
         await _discountRepository.DeleteAdvertDiscounts(request.AdvertId);

         return new Empty();
     }
 }