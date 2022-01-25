using System;
using System.Reflection;

namespace Sean.Core.DependencyInjection
{
    public interface IDIContainer
    {
        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="style"></param>
        void RegisterType<TService, TImplementation>(ServiceLifeStyle style);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="style"></param>
        void RegisterType<TService>(ServiceLifeStyle style);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="style"></param>
        void RegisterType(Type serviceType, Type implementationType, ServiceLifeStyle style);

        /// <summary>
        /// 注册指定程序集中符合条件的所有类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="interfaceSuffix">接口后缀，如果：Service、Repository等</param>
        /// <param name="style"></param>
        /// <param name="filter"></param>
        void RegisterAssemblyByInterfaceSuffix(Assembly assembly, string interfaceSuffix, ServiceLifeStyle style, Func<Type, Type, bool> filter = null);

        /// <summary>
        /// 构造函数注入解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        /// 构造函数注入解析
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object Resolve(Type serviceType);
    }
}