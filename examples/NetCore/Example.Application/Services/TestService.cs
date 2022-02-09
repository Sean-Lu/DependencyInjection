using System;
using Example.Application.Contracts;
using Example.Domain.Contracts;
using Example.Domain.Entities;
using Sean.Utility.Contracts;

namespace Example.Application.Services
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

            _testRepository.Hello(new TestEntity
            {
                Id = 100010,
                CreateTime = DateTime.Now
            });
        }
    }
}
