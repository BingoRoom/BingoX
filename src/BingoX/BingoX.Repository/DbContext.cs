using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BingoX.Repository
{
    /// <summary>
    /// 数据库上下文接口
    /// </summary>
    public interface IDbContext
    {

    }
    /// <summary>
    /// 数据库上下文抽象类
    /// </summary>
    public abstract class DbContext: IDbContext
    {

        public DbContext()
        {

        }
      
        public abstract void Close();
    }
}
