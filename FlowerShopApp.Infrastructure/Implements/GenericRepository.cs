using FlowerShopApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopApp.Infrastructure.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly FlowerShopAppContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(FlowerShopAppContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> Entities => _dbSet;

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}