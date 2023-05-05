using Microsoft.EntityFrameworkCore;
using MVCAngularShortener.Data;
using MVCAngularShortener.Data.Interfaces;

namespace MVCAngularShortener.Infrastructure.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger<RepositoryBase<T>> _logger;

        public RepositoryBase(ApplicationDbContext dbContext, ILogger<RepositoryBase<T>> logger)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
            _logger = logger;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Entity created: {Entity}", entity);

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Entity deleted: {Entity}", entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            _logger.LogInformation("Getting all entities.");

            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int? id)
        {
            if (id.HasValue && id.Value != 0)
            {
                var entity = await _dbSet.FindAsync(id);
                _logger.LogInformation("Entity found by ID: {Id}, Entity: {Entity}", id, entity);
                return entity;
            }
            else
            {
                _logger.LogInformation("Invalid ID provided: {Id}", id);
                return null;
            }
        }

        public Task UpdateAsync(int id, T entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChangesAsync();

            _logger.LogInformation("Entity updated: {Entity}", entity);

            return Task.CompletedTask;
        }
    }
}
