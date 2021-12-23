
#if NETCOREAPP3_1
using BingoX.AspNetCore;


namespace BingoX.DynamicSearch
{

    class ApiFeatureProvider : Microsoft.AspNetCore.Mvc.Controllers.ControllerFeatureProvider
    {
        protected override bool IsController(System.Reflection.TypeInfo typeInfo)
        {
            if (!typeof(IService).IsAssignableFrom(typeInfo) ||
                !typeInfo.IsPublic ||
                typeInfo.IsAbstract ||
                typeInfo.IsGenericType)
                return false;

            return true;
        }
    }

}
#endif