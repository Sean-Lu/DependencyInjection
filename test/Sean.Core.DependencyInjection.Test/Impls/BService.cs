using System;
using System.Collections.Generic;
using System.Text;
using Sean.Core.DependencyInjection.Test.Contracts;
using Sean.Utility.Contracts;

namespace Sean.Core.DependencyInjection.Test.Impls
{
    public class BService : IBService
    {
        private readonly ILogger _logger;
        private readonly ICService _cService;

        public BService(
            ILogger<BService> logger,
            ICService cService
            )
        {
            _logger = logger;
            _cService = cService;
        }
    }
}
