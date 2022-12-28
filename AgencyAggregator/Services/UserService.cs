using AgencyAggregator.Extensions;
using AgencyAggregator.Interfaces;
using AgencyAggregator.Models;
using Polly;
using Polly.Retry;

namespace AgencyAggregator.Services;

public class UserService : IUserService
{
    private readonly HttpClient _client;
    
    private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy =
        Policy.HandleResult<HttpResponseMessage>(message => (int)message.StatusCode == 503)
            .RetryAsync(3);

    public UserService(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<UserModel?> GetUser(string id)
    {
        var response = await RetryPolicy.ExecuteAsync(() =>
            _client.GetAsync($"/api/User/{id}"));
        
        return await response.ReadContentAs<UserModel>();
    }
}