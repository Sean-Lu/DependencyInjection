using Example.NetCore.Entities;

namespace Example.NetCore.Contracts
{
    public interface ITestRepository : IBaseRepository<TestEntity>
    {
        void Add(TestEntity entity);
    }
}