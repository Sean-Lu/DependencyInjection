using System;
using System.Collections.Generic;
using System.Text;
using Sean.Core.DependencyInjection.Test.Contracts;
using Sean.Utility.Contracts;

namespace Sean.Core.DependencyInjection.Test.Impls
{
    public class AService : IAService
    {
        private readonly ILogger _logger;
        private readonly IBService _bService;

        public AService(
            ILogger<AService> logger,
            IBService bService
            )
        {
            _logger = logger;
            _bService = bService;
        }
    }
}
