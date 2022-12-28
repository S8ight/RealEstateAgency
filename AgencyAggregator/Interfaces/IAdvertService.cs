using AgencyAggregator.Models;

namespace AgencyAggregator.Interfaces;

public interface IAdvertService
{ 
    Task<IEnumerable<AdvertModel>?> GetAdverts(string id);
}