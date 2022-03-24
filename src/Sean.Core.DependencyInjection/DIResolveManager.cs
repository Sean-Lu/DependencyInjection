using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sean.Core.DependencyInjection
{
    internal class DIResolveManager
    {
        /// <summary>
        /// 构造函数注入解析
        /// </summary>
        /// <param name="container"></param>
        /// <param name="serviceType"></param>
        /// <param name="diObservers"></param>
        /// <returns></returns>
        public static object Resolve(IDIContainer container, Type serviceType, LinkedList<DIObserver> diObservers = null)
        {
            object result;
            DIImpl diImpl;
            if (serviceType.IsGenericType && !serviceType.IsGenericTypeDefinition)
            {
                // 泛型解析
                var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
                if (!container.DITypeMap.TryGetValue(genericTypeDefinition, out diImpl) || diImpl == null)
                {
                    throw new UnresolvedTypeException(genericTypeDefinition, $"Unresolved type: {genericTypeDefinition.FullName}");
                }
            }
            else
            {
                if (!container.DITypeMap.TryGetValue(serviceType, out diImpl) || diImpl == null)
                {
                    throw new UnresolvedTypeException(serviceType, $"Unresolved type: {serviceType.FullName}");
                }
            }

            if (diImpl.ImplementationInstance != null)
            {
                return diImpl.ImplementationInstance;
            }
            if (diImpl.ImplementationFactory != null)
            {
                return diImpl.ImplementationFactory(container);
            }

            if (diImpl.LifeStyle == ServiceLifeStyle.Singleton)
            {
                if (DICacheManager.GetSingletonInstanceObject(serviceType, out result) && result != null)// 先尝试从单例实例缓存中获取
                {
                    return result;
                }
            }

            Type implementationType;
            if (serviceType.IsGenericType)
            {
                var genericArguments = serviceType.GetGenericArguments();
                implementationType = diImpl.ImplementationType.MakeGenericType(genericArguments);
            }
            else
            {
                implementationType = diImpl.ImplementationType;
            }

            #region 递归解析构造函数参数
            var ctorArray = implementationType.GetConstructors();
            List<object> ctorParaList = null;
            if (ctorArray.Length <= 1)
            {
                var ctor = ctorArray.FirstOrDefault();
                ctorParaList = ResolveConstructorParameters(container, ctor, diObservers);
            }
            else
            {
                var ctor = ctorArray.FirstOrDefault(c => c.IsDefined(typeof(DependencyInjectionAttribute), true))
                           ?? ctorArray.FirstOrDefault(c => c.GetParameters().Length == 0);// 无参构造函数
                if (ctor != null)
                {
                    ctorParaList = ResolveConstructorParameters(container, ctor, diObservers);
                }
                else
                {
                    // 按参数数量倒序遍历全部有参构造函数，直到找到可以提供所有参数的构造函数（优先解析参数数量最多的构造函数）。
                    var ctors = ctorArray.OrderByDescending(c => c.GetParameters().Length).ToList();
                    for (var i = 0; i < ctors.Count; i++)
                    {
                        var constructorInfo = ctors[i];
                        try
                        {
                            ctorParaList = ResolveConstructorParameters(container, constructorInfo, diObservers);
                            break;
                        }
                        catch (UnresolvedTypeException)
                        {
                            if (i + 1 >= ctors.Count)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            #endregion

            result = Activator.CreateInstance(implementationType, ctorParaList?.ToArray());

            if (diImpl.LifeStyle == ServiceLifeStyle.Singleton)
            {
                DICacheManager.AddOrUpdateSingletonInstanceObject(serviceType, result);// 更新单例实例缓存
            }

            return result;
        }

        private static List<object> ResolveConstructorParameters(IDIContainer container, ConstructorInfo ctor, LinkedList<DIObserver> diObservers = null)
        {
            if (ctor == null) return null;

            var parameters = ctor.GetParameters();
            if (!parameters.Any())
            {
                return null;
            }

            var ctorParaList = new List<object>();
            foreach (var para in parameters)
            {
                var paraServiceType = para.ParameterType;

                #region 链表：记录依赖解析过程
                LinkedListNode<DIObserver> curLinkedListNode = null;
                if (diObservers != null)
                {
                    var type = paraServiceType.IsGenericType && !paraServiceType.IsGenericTypeDefinition
                        ? paraServiceType.GetGenericTypeDefinition()
                        : paraServiceType;
                    if (diObservers.Any(c => c.ServiceType == type))
                    {
                        var listObserver = new List<string>();
                        foreach (var diObserver in diObservers)
                        {
                            listObserver.Add(diObserver.ServiceType.Name);
                        }
                        listObserver.Add(type.Name);
                        throw new CircularDependencyException($"A circular dependency was detected for the service of type '{type.FullName}', the complete process of dependency resolution: [{string.Join(" => ", listObserver)}]");
                    }
                    curLinkedListNode = diObservers.AddLast(new DIObserver
                    {
                        ServiceType = type
                    });
                }
                #endregion

                var oPara = Resolve(container, paraServiceType, diObservers);

                #region 链表：移除依赖解析过程
                if (diObservers != null)
                {
                    RemoveAfterNode(diObservers, curLinkedListNode);
                }
                #endregion

                ctorParaList.Add(oPara);
            }
            return ctorParaList;
        }

        private static void RemoveAfterNode(LinkedList<DIObserver> diObservers, LinkedListNode<DIObserver> node)
        {
            if (node == null)
            {
                return;
            }

            var delList = new List<LinkedListNode<DIObserver>> { node };
            while ((node = node.Next) != null)
            {
                delList.Add(node);
            }
            delList.ForEach(diObservers.Remove);
        }
    }
}
