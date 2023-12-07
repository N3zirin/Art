using Art.Abstractions.IRepositories;
using Art.Data;
using Art.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Art.Implementations.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataContext _dataContext;
        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public DbSet<T> Table => _dataContext.Set<T>();

        public async Task<bool> AddAsync(T data)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(data);
            return entityEntry.State == EntityState.Added;
        }

        public IQueryable<T> GetAll()
        {
            var query = Table.AsQueryable();//extension methods bir namespace de islemeye permit
            return query;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var query = Table.AsQueryable();

            return await query.FirstOrDefaultAsync(data => data.Id == id);
        }

        public bool Remove(T data)
        {
            EntityEntry<T> entityEntry = Table.Remove(data);
            return entityEntry.State == EntityState.Deleted;//state-tekce secilen entity set/get edir
        }

        public async Task<bool> RemoveById(int id)
        {
            T data = await Table.FindAsync(id);
            return Remove(data);
        }

        public bool Update(T data)
        {
            EntityEntry<T> entityEntry = Table.Update(data);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
