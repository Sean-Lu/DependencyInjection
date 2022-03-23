namespace Sean.Core.DependencyInjection
{
    /// <summary>
    /// 生命周期类型
    /// </summary>
    public enum ServiceLifeStyle
    {
        /// <summary>
        /// 瞬时
        /// </summary>
        Transient = 0,
        /// <summary>
        /// 作用域
        /// </summary>
        //Scoped = 1,
        /// <summary>
        /// 单例
        /// </summary>
        Singleton = 2
    }
}
