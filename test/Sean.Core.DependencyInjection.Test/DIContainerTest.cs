using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sean.Core.DependencyInjection.Test.Contracts;

namespace Sean.Core.DependencyInjection.Test
{
    [TestClass]
    public class DIContainerTest : TestBase
    {
        /// <summary>
        /// 循环依赖
        /// </summary>
        [TestMethod]
        public void CircularDependencyResolveTest()
        {
            Assert.ThrowsException<CircularDependencyException>(() =>
            {
                IAService aService = DependencyManager.Container.Resolve<IAService>();
            });
        }
    }
}