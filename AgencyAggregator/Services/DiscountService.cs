using AgencyAggregator.Extensions;
using AgencyAggregator.Interfaces;
using AgencyAggregator.Models;

namespace AgencyAggregator.Services;

public class DiscountAggService : IDiscountService
{
    private readonly HttpClient _client;

    public DiscountAggService(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<DiscountModel?> CheckForDiscount(string id)
    {
        var response = await _client.GetAsync($"/api/Discount/{id}");
        return await response.ReadContentAs<DiscountModel>();    }
}