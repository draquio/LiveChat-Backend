namespace LiveChat.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetById(int id);
        Task<List<T>> GetAll(int page, int pageSize);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
