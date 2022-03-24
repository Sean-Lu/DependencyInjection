using System;
using Example.Domain.Contracts;
using Example.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sean.Core.DbRepository;
using Sean.Utility.Contracts;

namespace Example.Domain.Repositories
{
    public class TestRepository : BaseRepository<TestEntity>, ITestRepository
    {
        private readonly ILogger _logger;

        public TestRepository(
            ILogger<TestRepository> logger,
            IConfiguration configuration
            ) : base(configuration)
        {
            _logger = logger;
        }

        public override void OutputExecutedSql(string sql, object param)
        {
            _logger.LogInfo($"执行了SQL：{sql}{Environment.NewLine}入参：{JsonConvert.SerializeObject(param, Formatting.Indented)}");
        }

        public void Hello()
        {
            var tableName = TableName();
            var isTableExists = IsTableExists(tableName);
            _logger.LogInfo($"表[{tableName}]是否存在：{isTableExists}");
        }
    }
}
