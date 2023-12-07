using Art.Abstractions.IRepositories.IPicturesRepos;
using Art.Data;
using Art.Entities;

namespace Art.Implementations.Repositories.EntityRepos
{
    public class PictureRepository : Repository<Picture>, IPictureRepository
    {
        public PictureRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
