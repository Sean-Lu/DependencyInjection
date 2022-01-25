using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sean.Core.DependencyInjection
{
    public class DIContainer : IDIContainer
    {
        private readonly ConcurrentDictionary<Type, DIObject> _diTypeDictionary = new();

        private static readonly ConcurrentDictionary<Type, object> _diSingletonDictionary = new();

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="style"></param>
        public void RegisterType<TService, TImplementation>(ServiceLifeStyle style)
        {
            var obj = new DIObject
            {
                ImplementationType = typeof(TImplementation),
                LifeStyle = style
            };
            _diTypeDictionary.AddOrUpdate(typeof(TService), obj, (key, value) => obj);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="style"></param>
        public void RegisterType<TService>(ServiceLifeStyle style)
        {
            var obj = new DIObject
            {
                ImplementationType = typeof(TService),
                LifeStyle = style
            };
            _diTypeDictionary.AddOrUpdate(typeof(TService), obj, (key, value) => obj);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="style"></param>
        public void RegisterType(Type serviceType, Type implementationType, ServiceLifeStyle style)
        {
            var obj = new DIObject
            {
                ImplementationType = implementationType,
                LifeStyle = style
            };
            _diTypeDictionary.AddOrUpdate(serviceType, obj, (key, value) => obj);
        }

        /// <summary>
        /// 注册指定程序集中符合条件的所有类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="interfaceSuffix">接口后缀，如果：Service、Repository等</param>
        /// <param name="style"></param>
        /// <param name="filter"></param>
        public void RegisterAssemblyByInterfaceSuffix(Assembly assembly, string interfaceSuffix, ServiceLifeStyle style, Func<Type, Type, bool> filter = null)
        {
            var types = assembly.GetTypes();
            var interfaceTypes = types.Where(c => c.IsInterface && c.Name.EndsWith(interfaceSuffix));
            foreach (var interfaceType in interfaceTypes)
            {
                var implType = types.FirstOrDefault(c => c.Name == interfaceType.Name.Substring(1));
                if (implType != null)
                {
                    if (filter != null && !filter(interfaceType, implType))
                    {
                        continue;
                    }
                    RegisterType(interfaceType, implType, style);
                }
            }
        }

        /// <summary>
        /// 构造函数注入解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// 构造函数注入解析
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object Resolve(Type serviceType)
        {
            object result;
            DIObject diObject;
            Type[] genericArguments = null;
            if (serviceType.IsGenericType)
            {
                // 泛型解析
                var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
                if (!_diTypeDictionary.TryGetValue(genericTypeDefinition, out diObject) || diObject == null)
                {
                    throw new InvalidOperationException($"Unresolved type: {genericTypeDefinition.FullName}");
                }
                genericArguments = serviceType.GetGenericArguments();
            }
            else
            {
                if (!_diTypeDictionary.TryGetValue(serviceType, out diObject) || diObject == null)
                {
                    throw new InvalidOperationException($"Unresolved type: {serviceType.FullName}");
                }
            }

            if (diObject.LifeStyle == ServiceLifeStyle.Singleton)
            {
                if (_diSingletonDictionary.TryGetValue(serviceType, out result) && result != null)
                {
                    return result;
                }
            }

            var implementationType = diObject.ImplementationType;
            var ctorArray = implementationType.GetConstructors();
            var ctor = ctorArray.Count(c => c.IsDefined(typeof(DependencyInjectionAttribute), true)) > 0
                ? ctorArray.FirstOrDefault(c => c.IsDefined(typeof(DependencyInjectionAttribute), true))
                : ctorArray.OrderBy(c => c.GetParameters().Length).FirstOrDefault();

            var paraList = new List<object>();
            if (ctor is not null)
            {
                foreach (var para in ctor.GetParameters())
                {
                    var paraInterfaceType = para.ParameterType;
                    var oPara = Resolve(paraInterfaceType);
                    paraList.Add(oPara);
                }
            }

            if (serviceType.IsGenericType && genericArguments is not null)
            {
                implementationType = implementationType.MakeGenericType(genericArguments);
            }
            result = Activator.CreateInstance(implementationType, paraList.ToArray());
            if (diObject.LifeStyle == ServiceLifeStyle.Singleton)
            {
                _diSingletonDictionary.AddOrUpdate(serviceType, result, (key, value) => result);
            }
            return result;
        }
    }
}