using Art.Abstractions.IRepositories;
using Art.Entities;

namespace Art.Abstractions.IUnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;//generic yaradiriq, umumi olsun deye 
        Task<int> SaveChangesAsync();//affected rows sayi ucun
    }
}
