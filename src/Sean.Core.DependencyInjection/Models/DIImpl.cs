using System;

namespace Sean.Core.DependencyInjection
{
    public class DIImpl
    {
        /// <summary>
        /// 服务实现实例
        /// </summary>
        public object ImplementationInstance { get; set; }
        /// <summary>
        /// 服务实现工厂
        /// </summary>
        public Func<IDIContainer, object> ImplementationFactory { get; set; }
        /// <summary>
        /// 服务实现类型
        /// </summary>
        public Type ImplementationType { get; set; }
        /// <summary>
        /// 服务生命周期
        /// </summary>
        public ServiceLifeStyle LifeStyle { get; set; }
    }
}
