using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class OCRBankCard : OCR
    {

        [JsonProperty("number")]
        public string Number { get; set; }
    }
}







