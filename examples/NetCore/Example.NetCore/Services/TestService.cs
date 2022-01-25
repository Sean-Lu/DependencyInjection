using System;
using System.Collections.Generic;
using System.Text;
using Example.NetCore.Contracts;
using Example.NetCore.Entities;
using Sean.Utility.Contracts;

namespace Example.NetCore.Services
{
    public class TestService : ITestService
    {
        private readonly ILogger _logger;
        private readonly ITestRepository _testRepository;

        public TestService(
            ILogger<TestService> logger,
            ITestRepository testRepository
            )
        {
            _logger = logger;
            _testRepository = testRepository;
        }

        public void Hello(string name)
        {
            _logger.LogInfo($"你好！{name}");

            _testRepository.Add(new TestEntity
            {
                Id = 100010,
                CreateTime = DateTime.Now
            });
        }
    }
}
