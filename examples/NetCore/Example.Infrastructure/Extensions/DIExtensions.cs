﻿using Sean.Core.DependencyInjection;
using Sean.Utility.Contracts;
using Sean.Utility.Impls.Log;

namespace Example.Infrastructure.Extensions
{
    public static class DIExtensions
    {
        /// <summary>
        /// 基础设施层依赖注入
        /// </summary>
        /// <param name="container"></param>
        public static void AddInfrastructureDI(this IDIRegister container)
        {
            // Logger注入
            //container.RegisterType<ILogger, SimpleLocalLogger>(ServiceLifeStyle.Singleton);
            container.RegisterType(typeof(ILogger<>), typeof(SimpleLocalLogger<>), ServiceLifeStyle.Transient);// 泛型注入
        }
    }
}
