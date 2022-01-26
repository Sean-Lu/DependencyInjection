using Example.NetCore.Contracts;

namespace Example.NetCore.Repositories
{
    public abstract class BaseRepository<TEntity> : BaseRepository, IBaseRepository<TEntity>
    {
    }

    public abstract class BaseRepository : IBaseRepository
    {
    }
}