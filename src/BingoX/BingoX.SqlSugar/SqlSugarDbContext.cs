﻿using BingoX.DataAccessor;
using BingoX.Repository;
using SqlSugar;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// 服务提供其在辅助数据字典中的键名
        /// </summary>
        internal const string DIConst = "ServiceProvider";
        /// <summary>
        /// 数据库上下文辅助数据字典
        /// </summary>
        public IDictionary<string, object> RootContextData { get; private set; } = new Dictionary<string, object>();
        /// <summary>
        /// 设置服务提供器
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void SetServiceProvider(System.IServiceProvider serviceProvider)
        {
            if (RootContextData.ContainsKey(DIConst)) RootContextData[DIConst] = serviceProvider;
            RootContextData.Add(DIConst, serviceProvider);
        }
        /// <summary>
        /// 获取服务提供器
        /// </summary>
        /// <returns></returns>
        public IServiceProvider GetServiceProvider()
        {
            return RootContextData.ContainsKey(DIConst) ? RootContextData[DIConst] as System.IServiceProvider : null;
        }

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
