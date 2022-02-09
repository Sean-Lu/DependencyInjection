using Example.Domain.Contracts;
using Example.Domain.Entities;
using Newtonsoft.Json;
using Sean.Core.DbRepository.Dapper.Impls;
using Sean.Utility.Contracts;

namespace Example.Domain.Repositories
{
    public class TestRepository : BaseRepository<TestEntity>, ITestRepository
    {
        private readonly ILogger _logger;

        public TestRepository(ILogger<TestRepository> logger)
        {
            _logger = logger;
        }

        public void Hello(TestEntity entity)
        {
            //Add(entity, true);

            _logger.LogInfo($"新增数据：{JsonConvert.SerializeObject(entity)}");
        }
    }
}
