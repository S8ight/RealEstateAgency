using AgencyAggregator.Models;

namespace AgencyAggregator.Interfaces;

public interface IDiscountService
{
    Task<DiscountModel?> CheckForDiscount(string id);
}