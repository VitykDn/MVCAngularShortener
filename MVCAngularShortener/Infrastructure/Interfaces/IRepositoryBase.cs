namespace MVCAngularShortener.Data.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int? id);
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(T entity);
        Task<T> CreateAsync(T entity);
    }
}
