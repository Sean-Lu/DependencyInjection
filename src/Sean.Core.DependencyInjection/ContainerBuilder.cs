using System;
using System.Collections.Generic;
using System.Text;

namespace Sean.Core.DependencyInjection
{
    public class ContainerBuilder
    {
        public IDIContainer Build()
        {
            return new DIContainer();
        }
    }
}
