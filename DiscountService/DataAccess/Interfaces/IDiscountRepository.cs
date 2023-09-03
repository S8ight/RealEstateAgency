using DiscountService.Entities;

namespace DiscountService.DataAccess.Interfaces;

public interface IDiscountRepository
{
    Task<Discount?> GetAdvertCurrentDiscount(string id);
    Task<IEnumerable<Discount>> GetAdvertDiscounts(string advertId);
    Task<Discount> GetDiscountById(string id);
    Task AddDiscount(Discount coupon);
    Task DeleteAdvertDiscounts(string advertId);
    Task DeleteDiscount(string id);
}