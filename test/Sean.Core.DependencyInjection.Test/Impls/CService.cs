using System;
using System.Collections.Generic;
using System.Text;
using Sean.Core.DependencyInjection.Test.Contracts;

namespace Sean.Core.DependencyInjection.Test.Impls
{
    public class CService : ICService
    {
        private readonly IAService _aService;

        public CService(
            IAService aService
            )
        {
            _aService = aService;
        }
    }
}
