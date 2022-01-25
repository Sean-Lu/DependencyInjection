using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Example.NetCore.Contracts;
using Example.NetCore.Repositories;
using Example.NetCore.Services;
using Sean.Core.DependencyInjection;
using Sean.Utility.Contracts;
using Sean.Utility.Impls.Log;

namespace Example.NetCore
{
    public class DependencyManager
    {
        public static IDIContainer Container { get; private set; }

        public static void Register()
        {
            var builder = new ContainerBuilder();

            var container = builder.Build();

            // Logger注入
            //container.RegisterType<ILogger, SimpleLocalLogger>(ServiceLifeStyle.Singleton);
            container.RegisterType(typeof(ILogger<>), typeof(SimpleLocalLogger<>), ServiceLifeStyle.Transient);// 泛型注入

            // Repositories注入
            //container.RegisterType<ITestRepository, TestRepository>(ServiceLifeStyle.Transient);
            container.RegisterAssemblyByInterfaceSuffix(Assembly.GetCallingAssembly(), "Repository", ServiceLifeStyle.Transient);

            // Services注入
            //container.RegisterType<ITestService, TestService>(ServiceLifeStyle.Transient);
            container.RegisterAssemblyByInterfaceSuffix(Assembly.GetCallingAssembly(), "Service", ServiceLifeStyle.Transient);

            Container = container;
        }
    }
}
