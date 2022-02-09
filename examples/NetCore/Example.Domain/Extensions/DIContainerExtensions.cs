using System.Reflection;
using Example.Domain.Contracts;
using Example.Domain.Repositories;
using Sean.Core.DbRepository.Contracts;
using Sean.Core.DependencyInjection;

namespace Example.Domain.Extensions
{
    public static class DIContainerExtensions
    {
        /// <summary>
        /// 领域层依赖注入
        /// </summary>
        /// <param name="container"></param>
        public static void AddDomainDI(this IDIRegister container)
        {
            // Repositories注入
            //container.RegisterType<ITestRepository, TestRepository>(ServiceLifeStyle.Transient);
            container.RegisterAssemblyByInterfaceSuffix(Assembly.GetExecutingAssembly(), "Repository", ServiceLifeStyle.Transient);
            //container.RegisterAssemblyByInheritedInterfaceType(Assembly.GetExecutingAssembly(), typeof(IBaseRepository), ServiceLifeStyle.Transient);
            //container.RegisterAssemblyByInheritedInterfaceType(Assembly.GetExecutingAssembly(), typeof(IBaseRepository<>), ServiceLifeStyle.Transient);
        }
    }
}
