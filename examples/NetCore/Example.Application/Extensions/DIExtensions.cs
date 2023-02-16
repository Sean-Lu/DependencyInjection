using System.Reflection;
using Example.Application.Contracts;
using Example.Application.Contracts.Base;
using Example.Application.Services;
using Example.Domain.Extensions;
using Sean.Core.DependencyInjection;

namespace Example.Application.Extensions
{
    public static class DIExtensions
    {
        /// <summary>
        /// 应用层依赖注入
        /// </summary>
        /// <param name="container"></param>
        public static void AddApplicationDI(this IDIRegister container)
        {
            container.AddDomainDI();

            // Services注入
            //container.RegisterType<ITestService, TestService>(ServiceLifeStyle.Transient);
            container.RegisterByInterfaceSuffix(Assembly.GetExecutingAssembly(), "Service", ServiceLifeStyle.Transient);
            //container.RegisterByInheritedInterfaceType(Assembly.GetExecutingAssembly(), typeof(IBaseService), ServiceLifeStyle.Transient);
        }
    }
}
