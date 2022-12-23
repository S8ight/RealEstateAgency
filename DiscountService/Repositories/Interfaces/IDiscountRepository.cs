using DiscountService.Entities;

namespace DiscountService.Repositories.Interfaces;

public interface IDiscountRepository
{
    Task<Discount?> GetDiscount(string id);
    Task AddDiscount(Discount coupon);
    Task DeleteDiscount(string id);
}