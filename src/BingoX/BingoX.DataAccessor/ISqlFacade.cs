using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BingoX.DataAccessor
{

    public interface ISqlFacade
    {
        T Query<T>(string sqlcommand) where T : class, IEntity<T>;

        IList<T> QueryList<T>(string sqlcommand) where T : class, IEntity<T>;
        int ExecuteNonQuery(string sqlcommand);
        object ExecuteScalar(string sqlcommand);

        void TransactionExecute(IEnumerable<string> sqlcommands);
    }
}
