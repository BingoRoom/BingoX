using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoX.Comm.PaySDK
{
    public interface IAccessToken
    {
        string UserId { get; set; }
        string AccessToken { get; set; }
        long ExpiresIn { get; set; }
    }
}
