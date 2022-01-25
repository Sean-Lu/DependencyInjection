using System;
using System.Collections.Generic;
using System.Text;
using Example.NetCore.Contracts;
using Example.NetCore.Repositories;
using Example.NetCore.Services;
using Sean.Core.DependencyInjection;
using Sean.Utility.Contracts;
using Sean.Utility.Impls.Log;

namespace Example.NetCore.Impls
{
    /// <summary>
    /// 依赖注入测试
    /// </summary>
    public class DITest : ISimpleDo
    {
        public void Execute()
        {
            DependencyManager.Register();

            ITestService testService = DependencyManager.Container.Resolve<ITestService>();
            testService.Hello("Sean");
        }
    }
}
