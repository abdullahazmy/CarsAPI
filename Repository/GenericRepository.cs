using CarsAPI.Models;
using CarsAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarsAPI.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        // ✅ Inject `AppDbContext` correctly
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>(); // ✅ Set the correct DbSet
        }

        // ✅ Return `IReadOnlyList<T>` for safety
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }

        // Get a single field by a condition
        public async Task<T?> GetSingleByCondition(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).FirstOrDefaultAsync();
        }


        public async Task<T?> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }


        public async Task<T?> GetByIdAsync(string id, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<string>(e, "Id") == id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
