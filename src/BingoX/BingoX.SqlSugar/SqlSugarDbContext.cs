using BingoX.Repository;
using SqlSugar;
using System.Data;

namespace BingoX.SqlSugar
{
    /// <summary>
    /// SqlSugar数据库上下文
    /// </summary>
    public class SqlSugarDbContext : IDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public SqlSugarDbContext(ConnectionConfig config)
        {
            Client = new SqlSugarClient(config);
            SqlFacade = new SqlSugarSqlFacade(this);
        }
        /// <summary>
        /// SqlSugar客户端
        /// </summary>
        public SqlSugarClient Client { get; private set; }

        public SqlSugarSqlFacade SqlFacade { get; private set; }

        ISqlFacade IDbContext.SqlFacade { get { return SqlFacade; } }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            if (Client != null)
            {
                Client.Close();
                Client.Dispose();
                Client = null;
            }
        }
    }
}
