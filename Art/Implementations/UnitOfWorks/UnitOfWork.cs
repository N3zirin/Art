using Art.Abstractions.IRepositories;
using Art.Abstractions.IUnitOfWorks;
using Art.Data;
using Art.Entities;
using Art.Implementations.Repositories;
using Google;

namespace Art.Implementations.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        private readonly Dictionary<Type, object> _repositories;
        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext; 
            _repositories = new Dictionary<Type, object>();  
        }
        public void Dispose()
        {
            _dataContext.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            IRepository<TEntity> repository = new Repository<TEntity>(_dataContext);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }

    }
}
