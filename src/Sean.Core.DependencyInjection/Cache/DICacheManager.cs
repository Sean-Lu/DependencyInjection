using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sean.Core.DependencyInjection
{
    internal class DICacheManager
    {
        /// <summary>
        /// 单例实例缓存
        /// </summary>
        private static readonly ConcurrentDictionary<Type, object> _diSingletonInstanceCache = new();

        #region 单例实例缓存
        /// <summary>
        /// 获取单例实例缓存
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="singletonInstanceObject"></param>
        /// <returns></returns>
        public static bool GetSingletonInstanceObject(Type serviceType, out object singletonInstanceObject)
        {
            return _diSingletonInstanceCache.TryGetValue(serviceType, out singletonInstanceObject) && singletonInstanceObject != null;
        }
        /// <summary>
        /// 新增或更新单例实例缓存
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="singletonInstanceObject"></param>
        public static void AddOrUpdateSingletonInstanceObject(Type serviceType, object singletonInstanceObject)
        {
            _diSingletonInstanceCache.AddOrUpdate(serviceType, singletonInstanceObject, (key, value) => singletonInstanceObject);
        }
        #endregion
    }
}
