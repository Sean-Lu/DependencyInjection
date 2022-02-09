using System;
using System.Collections.Concurrent;

namespace Sean.Core.DependencyInjection
{
    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public interface IDIContainer : IDIRegister, IDIResolve
    {
        ConcurrentDictionary<Type, DIImpl> DITypeDictionary { get; }
    }
}