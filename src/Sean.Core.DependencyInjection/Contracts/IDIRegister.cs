using System;
using System.Reflection;

namespace Sean.Core.DependencyInjection
{
    public interface IDIRegister
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
        /// <typeparam name="TService"></typeparam>
        /// <param name="implementationType"></param>
        /// <param name="style"></param>
        void RegisterType<TService>(TService implementationType);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="func"></param>
        void RegisterType<TService>(Func<IDIContainer, object> func);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="func"></param>
        void RegisterType(Type type, Func<IDIContainer, object> func);

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
        /// <returns>成功注册的类型数量</returns>
        int RegisterAssemblyByInterfaceSuffix(Assembly assembly, string interfaceSuffix, ServiceLifeStyle style, Func<Type, Type, bool> filter = null);

        /// <summary>
        /// 注册指定程序集中符合条件的所有类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="inheritedInterfaceType">继承的接口类型</param>
        /// <param name="style"></param>
        /// <param name="filter"></param>
        /// <returns>成功注册的类型数量</returns>
        int RegisterAssemblyByInheritedInterfaceType(Assembly assembly, Type inheritedInterfaceType, ServiceLifeStyle style, Func<Type, Type, bool> filter = null);
    }
}