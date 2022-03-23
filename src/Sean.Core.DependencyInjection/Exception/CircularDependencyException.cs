using System;

namespace Sean.Core.DependencyInjection
{
    /// <summary>
    /// 循环依赖
    /// </summary>
    public class CircularDependencyException : Exception
    {
        public CircularDependencyException(string message) : base(message)
        {
        }
    }
}
