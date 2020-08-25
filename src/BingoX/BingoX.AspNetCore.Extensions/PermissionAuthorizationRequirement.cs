using Microsoft.AspNetCore.Authorization;

namespace BingoX.AspNetCore.Extensions
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
    }
}
