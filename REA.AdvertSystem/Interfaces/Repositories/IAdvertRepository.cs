using Ardalis.Specification;
using REA.AdvertSystem.DataAccess.Entities;

namespace REA.AdvertSystem.Interfaces.Repositories;

public interface IAdvertRepository
{
    IQueryable<Advert> GetAdverts();
    Task<Advert> GetAvertByIdAsync(string id);
    IQueryable<Advert> GetUserAdverts(string userId);
    Task AddAdvertAsync(Advert advert);
    Task UpdateAdvertAsync(Advert advert);
    Task DeleteAdvertAsync(string id);
    Task<List<Advert>> ListAsync(ISpecification<Advert> spec);
    Task<int> CountAsync(ISpecification<Advert> spec);
}