using AgencyAggregator.Extensions;
using AgencyAggregator.Interfaces;
using AgencyAggregator.Models;
using Polly;
using Polly.CircuitBreaker;

namespace AgencyAggregator.Services;

public class AdvertService : IAdvertService
{
    private readonly HttpClient _client;

    private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy =
        Policy.HandleResult<HttpResponseMessage>(message => (int)message.StatusCode == 503)
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

    public AdvertService(HttpClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<AdvertModel>?> GetAdverts(string id)
    {
        if (CircuitBreakerPolicy.CircuitState == CircuitState.Open)
        {
            throw new Exception("Service is currently unavailable");
        }

        var response = await CircuitBreakerPolicy.ExecuteAsync(() =>
            _client.GetAsync($"/api/Advert/User/{id}"));
        
        return await response.ReadContentAs<IEnumerable<AdvertModel>>();
    }
}