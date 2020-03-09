using System;
using System.Collections.Generic;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarPropertyValues
    {


        public SqlSugarPropertyValues(object entity)
        {
            this.entity = entity;
        }
        private readonly object entity;
        readonly IDictionary<string, object> current = new Dictionary<string, object>();
        public object this[string key]
        {
            get { throw new NotImplementedException(); }
            set
            {
            }
        }

        public string[] Properties { get; internal set; }

        public void SetValues(IDictionary<string, object> changeValues)
        {
            throw new NotImplementedException();
        }
    }
}
