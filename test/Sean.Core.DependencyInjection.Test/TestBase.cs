﻿using System;
using System.Collections.Generic;
using System.Text;
using Sean.Core.DependencyInjection.Test.Contracts;
using Sean.Core.DependencyInjection.Test.Impls;

namespace Sean.Core.DependencyInjection.Test
{
    public class TestBase
    {
        static TestBase()
        {
            DependencyManager.Register(container =>
            {
                container.RegisterType<IAService, AService>(ServiceLifeStyle.Transient);
                container.RegisterType<IBService, BService>(ServiceLifeStyle.Transient);
                container.RegisterType<ICService, CService>(ServiceLifeStyle.Transient);
            });
        }
    }
}
