using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    internal class NewtonsoftJsonSerializer : IDeserializer
    {






        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}







