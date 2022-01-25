using Example.NetCore.Entities;

namespace Example.NetCore.Contracts
{
    public interface ITestRepository
    {
        void Add(TestEntity entity);
    }
}