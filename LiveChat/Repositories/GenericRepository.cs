using LiveChat.Context;
using LiveChat.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiveChat.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDBContext _dbContext;

        public GenericRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T?> GetById(int id)
        {
            T? entity = await _dbContext.Set<T>().FindAsync(id);
            return entity;
        }
        public async Task<List<T>> GetAll(int page, int pageSize)
        {
            List<T> entities = await _dbContext.Set<T>()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return entities;
        }


        public async Task<T> Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<T> Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            int affected = await _dbContext.SaveChangesAsync();
            return affected > 0;
        }
    }
}
