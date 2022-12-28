using AgencyAggregator.Models;

namespace AgencyAggregator.Interfaces;

public interface IUserService
{
    Task<UserModel?> GetUser(string id);
}