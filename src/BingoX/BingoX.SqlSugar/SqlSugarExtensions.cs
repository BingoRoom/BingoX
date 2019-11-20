
using SqlSugar;

namespace BingoX.SqlSugar
{
#if Standard
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    public static class SqlSugarExtensions
    {
        public static void AddSqlSugar(this IServiceCollection services, IConfiguration configuration)
        {
            AddSqlSugar(services, configuration, "DefaultConnection", "Slave1Connection", "Slave2Connection");
        }
        public static void AddSqlSugar(this IServiceCollection services, IConfiguration configuration, string defaultConnection, params string[] slaveConnectionNames)
        {
            var slaveConnectionConfigs = GetDbSlaveConfig(configuration, slaveConnectionNames);

            var connectionconfig = new ConnectionConfig
            {
                ConnectionString = configuration.GetConnectionString(defaultConnection),
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                SlaveConnectionConfigs = slaveConnectionConfigs
            };
            if (string.IsNullOrEmpty(connectionconfig.ConnectionString)) throw new ApplicationException("数据库连接为空");
            services.AddSingleton(connectionconfig);
            services.AddScoped((n) =>
            {
                var config = n.GetService<ConnectionConfig>();
                var db = new SqlSugarDbContext(config);

                return db;
            });
        }

        static List<SlaveConnectionConfig> GetDbSlaveConfig(IConfiguration configuration, params string[] connectionName)
        {
            var slaveConnectionConfigs = new List<SlaveConnectionConfig>();//从库集合
            foreach (var item in connectionName)
            {

                var connStr = configuration.GetConnectionString(item);
                if (string.IsNullOrEmpty(connStr)) continue;
                slaveConnectionConfigs.Add(new SlaveConnectionConfig()
                {
                    HitRate = 10,
                    ConnectionString = connStr
                });
            }

            return slaveConnectionConfigs;
        }
    }
#endif
}
