using Art.Abstractions.IRepositories.IArtistRepos;
using Art.Data;
using Art.Entities;

namespace Art.Implementations.Repositories.EntityRepos
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        public ArtistRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
