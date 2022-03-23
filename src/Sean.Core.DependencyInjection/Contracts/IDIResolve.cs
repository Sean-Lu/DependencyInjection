using System;

namespace Sean.Core.DependencyInjection
{
    /// <summary>
    /// 依赖注入解析
    /// </summary>
    public interface IDIResolve
    {
        /// <summary>
        /// 构造函数注入解析
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        TService Resolve<TService>();

        /// <summary>
        /// 构造函数注入解析
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        object Resolve(Type serviceType);
    }
}