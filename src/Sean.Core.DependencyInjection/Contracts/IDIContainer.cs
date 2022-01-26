using System;
using System.Collections.Concurrent;

namespace Sean.Core.DependencyInjection
{
    public interface IDIContainer : IDIRegister, IDIResolve
    {
        ConcurrentDictionary<Type, DIImpl> DITypeDictionary { get; }
    }
}