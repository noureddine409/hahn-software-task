namespace API.Repositories
{
    public interface GenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
