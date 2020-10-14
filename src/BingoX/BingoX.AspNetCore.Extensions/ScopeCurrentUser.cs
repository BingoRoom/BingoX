using BingoX.AspNetCore;
using System.Security.Claims;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class DIExtensions
    {
        class ScopeCurrentUser : ICurrentUser
        {
            public object UserID { get; internal set; }

            public string Name { get; internal set; }

            public string Role { get; internal set; }

            public Claim[] Claims { get; internal set; }
            public bool IsAuthenticated { get; internal set; }
        }
    }
}
