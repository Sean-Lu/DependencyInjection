namespace Example.NetCore.Contracts
{
    public interface IBaseRepository
    {

    }

    public interface IBaseRepository<TEntity> : IBaseRepository
    {
    }
}