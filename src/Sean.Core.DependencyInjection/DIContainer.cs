using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sean.Core.DependencyInjection
{
    public class DIContainer : IDIContainer
    {
        public ConcurrentDictionary<Type, DIImpl> DITypeDictionary => _diTypeDictionary;

        private readonly ConcurrentDictionary<Type, DIImpl> _diTypeDictionary = new();
        private readonly ConcurrentDictionary<Type, Func<IDIContainer, object>> _diTypeFuncDictionary = new();

        private static readonly ConcurrentDictionary<Type, object> _diSingletonDictionary = new();

        #region 注册类型
        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="style"></param>
        public void RegisterType<TService, TImplementation>(ServiceLifeStyle style)
        {
            RegisterType(typeof(TService), typeof(TImplementation), style);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="style"></param>
        public void RegisterType<TService>(ServiceLifeStyle style)
        {
            RegisterType(typeof(TService), typeof(TService), style);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="implementationType"></param>
        /// <param name="style"></param>
        public void RegisterType<TService>(TService implementationType)
        {
            var obj = new DIImpl
            {
                ImplementationInstance = implementationType,
                ImplementationType = implementationType.GetType(),
                LifeStyle = ServiceLifeStyle.Singleton
            };
            _diTypeDictionary.AddOrUpdate(typeof(TService), obj, (key, value) => obj);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="func"></param>
        public void RegisterType<TService>(Func<IDIContainer, object> func)
        {
            RegisterType(typeof(TService), func);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="func"></param>
        public void RegisterType(Type type, Func<IDIContainer, object> func)
        {
            _diTypeFuncDictionary.AddOrUpdate(type, func, (key, value) => func);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="style"></param>
        public void RegisterType(Type serviceType, Type implementationType, ServiceLifeStyle style)
        {
            var obj = new DIImpl
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
        /// <returns>成功注册的类型数量</returns>
        public int RegisterAssemblyByInterfaceSuffix(Assembly assembly, string interfaceSuffix, ServiceLifeStyle style, Func<Type, Type, bool> filter = null)
        {
            if (string.IsNullOrWhiteSpace(interfaceSuffix))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(interfaceSuffix));

            var types = assembly.GetTypes();
            var interfaceTypes = types.Where(c => c.IsInterface && c.Name.EndsWith(interfaceSuffix)).ToList();
            return RegisterInterfaceTypes(interfaceTypes, types, style, filter);
        }

        /// <summary>
        /// 注册指定程序集中符合条件的所有类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="inheritedInterfaceType">继承的接口类型</param>
        /// <param name="style"></param>
        /// <param name="filter"></param>
        /// <returns>成功注册的类型数量</returns>
        public int RegisterAssemblyByInheritedInterfaceType(Assembly assembly, Type inheritedInterfaceType, ServiceLifeStyle style, Func<Type, Type, bool> filter = null)
        {
            if (inheritedInterfaceType == null || !inheritedInterfaceType.IsInterface)
            {
                return 0;
            }

            var types = assembly.GetTypes();
            var interfaceTypes = types.Where(c => c.IsInterface && c != inheritedInterfaceType && IsAssignableFrom(c, inheritedInterfaceType)).ToList();
            return RegisterInterfaceTypes(interfaceTypes, types, style, filter);
        }
        #endregion

        #region 解析类型
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
            DIImpl diImpl;
            Type[] genericArguments = null;
            if (serviceType.IsGenericType)
            {
                // 泛型解析
                var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
                if (_diTypeFuncDictionary.TryGetValue(genericTypeDefinition, out var func) && func != null)
                {
                    return func(this);
                }
                if (!_diTypeDictionary.TryGetValue(genericTypeDefinition, out diImpl) || diImpl == null)
                {
                    throw new UnresolvedTypeException(genericTypeDefinition, $"无法解析的类型：{genericTypeDefinition.FullName}");
                }
                genericArguments = serviceType.GetGenericArguments();
            }
            else
            {
                if (_diTypeFuncDictionary.TryGetValue(serviceType, out var func) && func != null)
                {
                    return func(this);
                }
                if (!_diTypeDictionary.TryGetValue(serviceType, out diImpl) || diImpl == null)
                {
                    throw new UnresolvedTypeException(serviceType, $"无法解析的类型：{serviceType.FullName}");
                }
            }

            if (diImpl.ImplementationInstance != null)
            {
                return diImpl.ImplementationInstance;
            }

            if (diImpl.LifeStyle == ServiceLifeStyle.Singleton)
            {
                if (_diSingletonDictionary.TryGetValue(serviceType, out result) && result != null)
                {
                    return result;
                }
            }

            var implementationType = diImpl.ImplementationType;
            var ctorArray = implementationType.GetConstructors();
            var ctor = ctorArray.Count(c => c.IsDefined(typeof(DependencyInjectionAttribute), true)) > 0
                ? ctorArray.FirstOrDefault(c => c.IsDefined(typeof(DependencyInjectionAttribute), true))
                : ctorArray.OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();

            var paraList = new List<object>();
            if (ctor != null)
            {
                foreach (var para in ctor.GetParameters())
                {
                    var paraInterfaceType = para.ParameterType;
                    var oPara = Resolve(paraInterfaceType);
                    paraList.Add(oPara);
                }
            }

            if (serviceType.IsGenericType && genericArguments != null)
            {
                implementationType = implementationType.MakeGenericType(genericArguments);
            }
            result = Activator.CreateInstance(implementationType, paraList.ToArray());
            if (diImpl.LifeStyle == ServiceLifeStyle.Singleton)
            {
                _diSingletonDictionary.AddOrUpdate(serviceType, result, (key, value) => result);
            }
            return result;
        }
        #endregion

        private bool IsAssignableFrom(Type implType, Type baseType)
        {
            if (implType == null || baseType == null)
            {
                return false;
            }

            if (baseType.IsGenericType && baseType.IsGenericTypeDefinition)
            {
                return IsAssignableFromGenericTypeDefinition(implType, baseType);
            }

            return baseType.IsAssignableFrom(implType);
        }
        private bool IsAssignableFromGenericTypeDefinition(Type implType, Type genericTypeDefinition)
        {
            if (genericTypeDefinition.IsInterface)
            {
                var interfaceTypes = implType.GetInterfaces();
                foreach (var interfaceType in interfaceTypes)
                {
                    if (interfaceType.IsGenericType &&
                        interfaceType.GetGenericTypeDefinition() == genericTypeDefinition)
                    {
                        return true;
                    }
                }

                return false;
            }

            var baseType = implType.BaseType;
            if (baseType == null || baseType == typeof(object)) return false;

            return baseType.IsGenericType &&
                   baseType.GetGenericTypeDefinition() == genericTypeDefinition ||
                   IsAssignableFromGenericTypeDefinition(baseType, genericTypeDefinition);
        }

        private int RegisterInterfaceTypes(IEnumerable<Type> interfaceTypes, Type[] types, ServiceLifeStyle style, Func<Type, Type, bool> filter = null)
        {
            var count = 0;
            foreach (var interfaceType in interfaceTypes)
            {
                var implType = types.FirstOrDefault(c => c.IsClass
                                                         && !c.IsAbstract
                                                         && c.Name == interfaceType.Name.Substring(1));
                if (implType != null)
                {
                    if (filter != null && !filter(interfaceType, implType))
                    {
                        continue;
                    }

                    count++;
                    RegisterType(interfaceType, implType, style);
                }
            }

            return count;
        }
    }
}