using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.DataAccessor
{
    public class DataAccessorBuilderInfoColletion : Collection<DataAccessorBuilderInfo>
    {
        public void Add(Action<DataAccessorBuilderInfo> option)
        {
            if (option == null) throw new ArgumentException("参数不能为空", nameof(option));
            var dataAccessorBuilderInfo = new DataAccessorBuilderInfo();
            option(dataAccessorBuilderInfo);
            Add(dataAccessorBuilderInfo);
        }
    }
}
