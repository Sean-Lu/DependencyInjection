using System;
using System.Collections.Generic;
using System.Text;
using Sean.Core.DependencyInjection.Test.Contracts;

namespace Sean.Core.DependencyInjection.Test.Impls
{
    public class BService : IBService
    {
        private readonly ICService _cService;

        public BService(
            ICService cService
            )
        {
            _cService = cService;
        }
    }
}
