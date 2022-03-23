using Example.Domain.Entities;
using Sean.Core.DbRepository;

namespace Example.Domain.Contracts
{
    public interface ITestRepository : IBaseRepository<TestEntity>
    {
        void Hello(TestEntity entity);
    }
}