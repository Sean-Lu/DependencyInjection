using System;
using System.Collections.Generic;
using System.Text;
using Example.NetCore.Contracts;
using Example.NetCore.Entities;
using Newtonsoft.Json;
using Sean.Utility.Contracts;

namespace Example.NetCore.Repositories
{
    public class TestRepository : BaseRepository<TestEntity>, ITestRepository
    {
        private readonly ILogger _logger;

        public TestRepository(ILogger<TestRepository> logger)
        {
            _logger = logger;
        }

        public void Add(TestEntity entity)
        {

            _logger.LogInfo($"新增数据：{JsonConvert.SerializeObject(entity)}");
        }
    }
}
