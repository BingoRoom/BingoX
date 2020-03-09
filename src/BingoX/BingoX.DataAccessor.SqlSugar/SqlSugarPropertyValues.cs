using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarPropertyValues
    {


        public SqlSugarPropertyValues(object entity)
        {
            this.entity = entity;
            propertiesMethods = entity.GetType().GetProperties().Where(n => n.CanWrite && n.CanWrite).ToArray();
            foreach (var item in propertiesMethods)
            {
                var value = BingoX.Helper.FastReflectionExtensions.FastGetValue(item, entity);
                current.Add(item.Name, value);
            }
        }
        private readonly object entity;
        private readonly PropertyInfo[] propertiesMethods;
        readonly IDictionary<string, object> current = new Dictionary<string, object>();
        public object this[string key]
        {
            get { return current.ContainsKey(key) ? current[key] : null; }
            set
            {
                if (current.ContainsKey(key)) current[key] = value;
            }
        }

        public string[] Properties { get; internal set; }

        public void SetValues(IDictionary<string, object> changeValues)
        {
            foreach (var item in changeValues)
            {
                var method = propertiesMethods.FirstOrDefault(n => string.Equals(n.Name, item.Key, StringComparison.InvariantCultureIgnoreCase));
                BingoX.Helper.FastReflectionExtensions.FastSetValue(method, entity, item.Value);

            }
        }
    }
}
