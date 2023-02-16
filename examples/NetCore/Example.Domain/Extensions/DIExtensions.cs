using System.Reflection;
using Example.Domain.Contracts;
using Example.Domain.Repositories;
using Example.Infrastructure.Extensions;
using MySql.Data.MySqlClient;
using Sean.Core.DbRepository;
using Sean.Core.DbRepository.Extensions;
using Sean.Core.DependencyInjection;

namespace Example.Domain.Extensions
{
    public static class DIExtensions
    {
        /// <summary>
        /// 领域层依赖注入
        /// </summary>
        /// <param name="container"></param>
        public static void AddDomainDI(this IDIRegister container)
        {
            container.AddInfrastructureDI();

            // Repositories注入
            //container.RegisterType<ITestRepository, TestRepository>(ServiceLifeStyle.Transient);
            container.RegisterByInterfaceSuffix(Assembly.GetExecutingAssembly(), "Repository", ServiceLifeStyle.Transient);
            //container.RegisterByInheritedInterfaceType(Assembly.GetExecutingAssembly(), typeof(IBaseRepository), ServiceLifeStyle.Transient);
            //container.RegisterByInheritedInterfaceType(Assembly.GetExecutingAssembly(), typeof(IBaseRepository<>), ServiceLifeStyle.Transient);

            #region Database configuration.

            #region 配置数据库和数据库提供者工厂之间的映射关系
            DatabaseType.MySql.SetDbProviderMap(new DbProviderMap("MySql.Data.MySqlClient", MySqlClientFactory.Instance));// MySql
            //DatabaseType.SqlServer.SetDbProviderMap(new DbProviderMap("System.Data.SqlClient", SqlClientFactory.Instance));// Microsoft SQL Server
            //DatabaseType.Oracle.SetDbProviderMap(new DbProviderMap("Oracle.ManagedDataAccess.Client", OracleClientFactory.Instance));// Oracle
            //DatabaseType.SQLite.SetDbProviderMap(new DbProviderMap("System.Data.SQLite", SQLiteFactory.Instance));// SQLite
            //DatabaseType.SQLite.SetDbProviderMap(new DbProviderMap("System.Data.SQLite", "System.Data.SQLite.SQLiteFactory,System.Data.SQLite"));// SQLite
            #endregion

            DbFactory.BulkCountLimit = 200;
            //DbFactory.OnSqlExecuting += OnSqlExecuting;
            //DbFactory.OnSqlExecuted += OnSqlExecuted;
            //DbFactory.JsonSerializer = NewJsonSerializer.Instance;

            #endregion
        }
    }
}
