using Newtonsoft.Json;
using System;

namespace BingoX.AspNetCore.Extensions
{
    public class EnumToStingJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var str = reader.Value != null ? reader.Value.ToString() : string.Empty;
            int value;
            if (int.TryParse(str, out value)) return Enum.ToObject(objectType, value);
            try
            {
                if (Enum.IsDefined(objectType, str)) return Enum.Parse(objectType, str);
            }
            catch (Exception)
            {


            }
            return Enum.ToObject(objectType, 0);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) return;
            writer.WriteValue(((int)value).ToString());
        }
    }
}
