using System;
using System.Collections.Concurrent;

namespace Sean.Core.DependencyInjection
{
    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public interface IDIContainer : IDIRegister, IDIResolve
    {
        /// <summary>
        /// 依赖注入类型映射关系
        /// </summary>
        ConcurrentDictionary<Type, DIImpl> DITypeMap { get; }
    }
}