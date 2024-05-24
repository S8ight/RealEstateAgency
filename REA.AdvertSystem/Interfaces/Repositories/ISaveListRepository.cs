using REA.AdvertSystem.DataAccess.Entities;

namespace REA.AdvertSystem.Interfaces.Repositories;

public interface ISaveListRepository
{
    IQueryable<SaveList> GetUserSaveList(string userId);
    Task AddSaveListAsync(SaveList saveList);
    Task DeleteSaveListAsync(string userId, string advertId);
}