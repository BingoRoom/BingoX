
using System;

namespace BingoX.EF
{
    public struct ModelMappingOption
    {
        public string ConfigAssemblyName { get; set; }

        public string EntityAssemblyName { get; set; }


        public Type BaseEntity { get; set; }

        public bool IsEffective()
        {
            if (string.IsNullOrEmpty(ConfigAssemblyName)) return false;
            if (string.IsNullOrEmpty(EntityAssemblyName)) return false;

            if (BaseEntity == null) return false;
            return true;
        }
    }


}
