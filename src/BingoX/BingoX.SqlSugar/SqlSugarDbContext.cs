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
        public SqlSugarDbContext(ConnectionConfig config)
        {
            Client = new SqlSugarClient(config);
        }
        /// <summary>
        /// SqlSugar客户端
        /// </summary>
        public SqlSugarClient Client { get; private set; }

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
