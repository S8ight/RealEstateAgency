namespace DiscountGrpcService.Repositories.Interfaces;

public interface IDiscountRepository
{
    Task<Discount.Discount?> GetDiscount(string id);
    Task AddDiscount(Discount.Discount coupon);
    Task DeleteDiscount(string id);
}