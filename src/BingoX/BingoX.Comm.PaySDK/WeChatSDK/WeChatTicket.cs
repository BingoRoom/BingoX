
using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
        public class WeChatTicket
        {
            [JsonProperty("ticket")]
            public string Ticket { get; set; }
            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

        }
    }
 


 
