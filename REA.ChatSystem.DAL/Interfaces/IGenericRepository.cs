namespace REA.ChatSystem.DAL.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(string id);
        Task<T> GetAsync(string id);
        Task<int> AddRangeAsync(IEnumerable<T> list);
        Task ReplaceAsync(T t);
        Task<string> AddAsync(T t);
    }
}
