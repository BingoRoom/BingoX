using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class OCRPrintedText : OCR
    {
        [JsonProperty("items")]
        public OCRPrintedTextItem[] Items { get; set; }
    }
    public class OCRPrintedTextItem
    {
        [JsonProperty("text")]
        public string Text { get; set; }

       
        [JsonProperty("pos")]
        public Post Post { get; set; }
    }
    public class Post
    {
        [JsonProperty("left_top")]
        public string TeftTop { get; set; }

        [JsonProperty("right_top")]
        public string RightTop { get; set; }

        [JsonProperty("right_bottom")]
        public string RightBottom { get; set; }

        [JsonProperty("left_bottom")]
        public string LeftBottom { get; set; }
    }
    public class Point
    {
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
    }
}







