using REA.AdvertSystem.DataAccess.Entities;

namespace REA.AdvertSystem.Interfaces.Repositories;

public interface IPhotoListRepository
{
    IQueryable<PhotoList> GetAdvertPhotoList(string advertId);
    Task<PhotoList?> GetPhotoListByIdAsync(string id);
    Task AddRangeOfPhotoListsAsync(List<PhotoList> photoLists);
    Task AddPhotoListAsync(PhotoList photoList);
    Task DeletePhotoListAsync(string id);
}