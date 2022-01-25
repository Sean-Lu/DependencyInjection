using System;
using Example.NetCore.Impls;
using Sean.Utility.Contracts;
using Sean.Utility.Extensions;
using Sean.Utility.Impls.Log;

namespace Example.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Logger配置
            SimpleLocalLoggerBase.DateTimeFormat = time => time.ToLongDateTime();
            SimpleLocalLoggerBase.DefaultLoggerOptions = new SimpleLocalLoggerOptions
            {
                LogToConsole = true,
                LogToLocalFile = false
            };
            #endregion

            ISimpleDo toDo = new DITest();
            toDo.Execute();

            Console.ReadLine();
        }
    }
}
