using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sean.Core.DependencyInjection
{
    public class DIContainer : IDIContainer
    {
        public ConcurrentDictionary<Type, DIImpl> DITypeMap => _diTypeMap;

        /// <summary>
        /// 依赖注入类型映射关系
        /// </summary>
        private readonly ConcurrentDictionary<Type, DIImpl> _diTypeMap = new();

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
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="style"></param>
        public void RegisterType<TImplementation>(ServiceLifeStyle style)
        {
            RegisterType(typeof(TImplementation), typeof(TImplementation), style);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="implementationInstance"></param>
        public void RegisterType<TService>(TService implementationInstance)
        {
            var obj = new DIImpl
            {
                ImplementationInstance = implementationInstance,
                ImplementationType = implementationInstance.GetType(),
                LifeStyle = ServiceLifeStyle.Singleton
            };
            _diTypeMap.AddOrUpdate(typeof(TService), obj, (key, value) => obj);
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
        /// <param name="serviceType"></param>
        /// <param name="func"></param>
        public void RegisterType(Type serviceType, Func<IDIContainer, object> func)
        {
            var obj = new DIImpl
            {
                ImplementationFactory = func,
                LifeStyle = ServiceLifeStyle.Transient
            };
            _diTypeMap.AddOrUpdate(serviceType, obj, (key, value) => obj);
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
            _diTypeMap.AddOrUpdate(serviceType, obj, (key, value) => obj);
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
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public TService Resolve<TService>()
        {
            return (TService)Resolve(typeof(TService));
        }

        /// <summary>
        /// 构造函数注入解析
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object Resolve(Type serviceType)
        {
            var diObservers = new LinkedList<DIObserver>();
            var type = serviceType.IsGenericType && !serviceType.IsGenericTypeDefinition
                ? serviceType.GetGenericTypeDefinition()
                : serviceType;
            diObservers.AddLast(new DIObserver
            {
                ServiceType = type
            });
            return DIResolveManager.Resolve(this, serviceType, diObservers);
        }
        #endregion

        #region Private method
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
        #endregion
    }
}