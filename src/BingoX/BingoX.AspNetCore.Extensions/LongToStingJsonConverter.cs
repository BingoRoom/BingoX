using Newtonsoft.Json;
using System;

namespace BingoX.AspNetCore.Extensions
{
    public class LongToStingJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(long) || objectType == typeof(long?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var str = reader.Value != null ? reader.Value.ToString() : string.Empty;
            long value;
            long.TryParse(str, out value);
            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) return;
            writer.WriteValue(((long)value).ToString());

        }
    }
}
