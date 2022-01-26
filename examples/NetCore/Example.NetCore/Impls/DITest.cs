using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Example.NetCore.Contracts;
using Example.NetCore.Repositories;
using Example.NetCore.Services;
using Microsoft.Extensions.Configuration;
using Sean.Core.DependencyInjection;
using Sean.Utility.Contracts;
using Sean.Utility.Impls.Log;

namespace Example.NetCore.Impls
{
    /// <summary>
    /// 依赖注入测试
    /// </summary>
    public class DITest : ISimpleDo
    {
        public void Execute()
        {
            #region 依赖注入
            DependencyManager.Register(container =>
            {
                // Configuration注入
                var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddEnvironmentVariables();
                var configuration = configurationBuilder.Build();
                container.RegisterType<IConfiguration>(configuration);
                //container.RegisterType<IConfiguration>(diContainer => configuration);

                // Logger注入
                //container.RegisterType<ILogger, SimpleLocalLogger>(ServiceLifeStyle.Singleton);
                container.RegisterType(typeof(ILogger<>), typeof(SimpleLocalLogger<>), ServiceLifeStyle.Transient);// 泛型注入

                // Repositories注入
                //container.RegisterType<ITestRepository, TestRepository>(ServiceLifeStyle.Transient);
                //container.RegisterAssemblyByInterfaceSuffix(Assembly.GetExecutingAssembly(), "Repository", ServiceLifeStyle.Transient);
                container.RegisterAssemblyByInheritedInterfaceType(Assembly.GetExecutingAssembly(), typeof(IBaseRepository), ServiceLifeStyle.Transient);
                //container.RegisterAssemblyByInheritedInterfaceType(Assembly.GetExecutingAssembly(), typeof(IBaseRepository<>), ServiceLifeStyle.Transient);

                // Services注入
                //container.RegisterType<ITestService, TestService>(ServiceLifeStyle.Transient);
                //container.RegisterAssemblyByInterfaceSuffix(Assembly.GetExecutingAssembly(), "Service", ServiceLifeStyle.Transient);
                container.RegisterAssemblyByInheritedInterfaceType(Assembly.GetExecutingAssembly(), typeof(IBaseService), ServiceLifeStyle.Transient);
            });
            #endregion

            ILogger logger = DependencyManager.Container.Resolve<ILogger<DITest>>();
            logger.LogInfo("这是一条测试日志");

            IConfiguration configuration = DependencyManager.Container.Resolve<IConfiguration>();
            var loggerSection = configuration.GetSection("SimpleLocalLoggerOptions");
            logger.LogInfo($"LogToConsole：{loggerSection.GetValue<bool>("LogToConsole")}");

            ITestService testService = DependencyManager.Container.Resolve<ITestService>();
            testService.Hello("Sean");
        }
    }
}
