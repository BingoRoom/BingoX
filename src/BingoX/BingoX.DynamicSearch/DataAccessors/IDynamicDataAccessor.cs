using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

namespace BingoX.DynamicSearch
{
    public interface IDynamicDataAccessor
    {
        string Node { get; }

    }
    public interface IWebRemoteDynamicDataAccessor
    {
        string Node { get; }
        string HostUrl { get; }



    }
    public interface IWebApiDynamicDataAccessor
    {
        object GetMany(Type classTye, string url, string method, NameValueCollection parameters);
        object[] GetOne(Type classTye, string url, string method, NameValueCollection parameters);

        DataTable GetTable(string url, string method, NameValueCollection parameters );
    }
    public interface IDbDynamicDataAccessor : IDynamicDataAccessor
    {
         
        long GetTotal(string sql, IDictionary<string, object> dataParameters);
        DataTable GetData(string sql, IDictionary<string, object> dataParameters);

        string GetSafeName(string fieldName);

        bool HasTable(string tablename);
        string[] GetFieldNames(string tablename);
    }

}
