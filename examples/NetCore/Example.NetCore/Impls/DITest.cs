using System;
using Example.Application.Contracts;
using Example.Application.Extensions;
using Example.Infrastructure;
using Microsoft.Extensions.Configuration;
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
            DIManager.Register(container =>
            {
                // Configuration注入
                var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddEnvironmentVariables();
                var configuration = configurationBuilder.Build();
                container.RegisterType<IConfiguration>(configuration);
                //container.RegisterType<IConfiguration>(diContainer => configuration);

                container.AddApplicationDI();
            });
            #endregion

            ILogger logger = DIManager.Resolve<ILogger<DITest>>();
            logger.LogInfo("这是一条测试日志");

            IConfiguration configuration = DIManager.Resolve<IConfiguration>();
            var loggerSection = configuration.GetSection("SimpleLocalLoggerOptions");
            logger.LogInfo($"Logger configuration: [{nameof(SimpleLocalLoggerOptions.LogToConsole)}:{loggerSection.GetValue<bool>(nameof(SimpleLocalLoggerOptions.LogToConsole))}] [{nameof(SimpleLocalLoggerOptions.LogToLocalFile)}:{loggerSection.GetValue<bool>(nameof(SimpleLocalLoggerOptions.LogToLocalFile))}]");

            ITestService testService = DIManager.Resolve<ITestService>();
            testService.Hello("靓仔！！！");
        }
    }
}
