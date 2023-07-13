using System;
using Example.Domain.Contracts;
using Example.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sean.Core.DbRepository;
using Sean.Core.DbRepository.Dapper;
using Sean.Utility.Contracts;

namespace Example.Domain.Repositories
{
    public class TestRepository : DapperBaseRepository<TestEntity>, ITestRepository
    {
        private readonly ILogger _logger;

        public TestRepository(
            ILogger<TestRepository> logger,
            IConfiguration configuration
            ) : base(configuration)
        {
            _logger = logger;
        }

        protected override void OnSqlExecuted(SqlExecutedContext context)
        {
            base.OnSqlExecuted(context);

            _logger.LogInfo($"执行了SQL：{context.Sql}{Environment.NewLine}入参：{JsonConvert.SerializeObject(context.SqlParameter, Formatting.Indented)}");
        }

        public void Hello()
        {
            var tableName = TableName();
            var isTableExists = IsTableExists(tableName);
            _logger.LogInfo($"[{tableName}]表是否存在：{isTableExists}");
        }
    }
}
