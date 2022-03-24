using System;
using System.Collections.Generic;
using System.Text;
using Sean.Core.DependencyInjection.Test.Contracts;
using Sean.Utility.Contracts;

namespace Sean.Core.DependencyInjection.Test.Impls
{
    public class CService : ICService
    {
        private readonly ILogger _logger;
        private readonly IAService _aService;

        public CService(
            ILogger<CService> logger,
            IAService aService
            )
        {
            _logger = logger;
            _aService = aService;
        }
    }
}
