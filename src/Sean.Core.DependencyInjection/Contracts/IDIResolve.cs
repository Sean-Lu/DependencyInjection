using System;

namespace Sean.Core.DependencyInjection
{
    public interface IDIResolve
    {
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
        object Resolve(Type serviceType);
    }
}