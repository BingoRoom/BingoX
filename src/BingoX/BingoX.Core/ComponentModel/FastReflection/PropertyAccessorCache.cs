using System.Reflection;

namespace BingoX.ComponentModel.FastReflection
{
    public class PropertyAccessorCache : FastReflectionCache<PropertyInfo, IPropertyAccessor>
    {
        protected override IPropertyAccessor Create(PropertyInfo key)
        {
            return new PropertyAccessor(key);
        }
    }
}
