namespace CarsAPI.Repository.Interfaces
{
    internal interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(string includeProperties = "");
        Task<T> GetByIdAsync(string id);
        Task<T?> GetByIdAsync(string id, string includeProperties = "");
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(string id);
    }
}