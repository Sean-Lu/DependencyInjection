using System;
using System.Collections.Generic;
using System.Text;
using Sean.Core.DependencyInjection.Test.Contracts;

namespace Sean.Core.DependencyInjection.Test.Impls
{
    public class AService : IAService
    {
        private readonly IBService _bService;

        public AService(
            IBService bService
            )
        {
            _bService = bService;
        }
    }
}
