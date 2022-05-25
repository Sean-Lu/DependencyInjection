using System;
using System.Reflection;

namespace Sean.Core.DependencyInjection
{
    /// <summary>
    /// 依赖注入注册
    /// </summary>
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
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="style"></param>
        void RegisterType<TImplementation>(ServiceLifeStyle style);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="implementationInstance"></param>
        void RegisterType<TService>(TService implementationInstance);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="func"></param>
        void RegisterType<TService>(Func<IDIContainer, object> func);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="func"></param>
        void RegisterType(Type serviceType, Func<IDIContainer, object> func);

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
        /// <param name="interfaceSuffix">接口后缀，如：Service、Repository</param>
        /// <param name="style"></param>
        /// <param name="filter"></param>
        /// <returns>成功注册的类型数量</returns>
        int RegisterByInterfaceSuffix(Assembly assembly, string interfaceSuffix, ServiceLifeStyle style, Func<Type, Type, bool> filter = null);

        /// <summary>
        /// 注册指定程序集中符合条件的所有类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="inheritedInterfaceType">继承的接口类型</param>
        /// <param name="style"></param>
        /// <param name="filter"></param>
        /// <returns>成功注册的类型数量</returns>
        int RegisterByInheritedInterfaceType(Assembly assembly, Type inheritedInterfaceType, ServiceLifeStyle style, Func<Type, Type, bool> filter = null);
    }
}