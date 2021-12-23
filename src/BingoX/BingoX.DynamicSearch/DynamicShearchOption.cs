
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

#if NETCOREAPP3_1
using Microsoft.AspNetCore.Mvc;
using BingoX.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
#endif

namespace BingoX.DynamicSearch
{
    public class DynamicShearchOption
    {

        public DynamicShearchOption()
        {
            Tables = new DynamicTableInfoCollection();
            DataAccessors = new DynamicDataAccessorCollection();
        }

        public DynamicTableInfoCollection Tables { get; private set; }
        public DynamicDataAccessorCollection DataAccessors { get; private set; }


        public void LoadConfig(string filePath)
        {
            DynamicSchemaReader reader = new DynamicSchemaReader(filePath);
            reader.LoadConfig();
            Tables = reader.Tables;
            DataAccessors = reader.DataAccessors;
        }
    }

}
