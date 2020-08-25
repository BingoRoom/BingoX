using Newtonsoft.Json;
using System;
using System.Globalization;

namespace BingoX.AspNetCore.Extensions
{
    public class DateJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }
        static readonly string[] formats = new string[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm", "yyyy-MM-dd" };
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(DateTime) || reader.ValueType == typeof(DateTime?))
            {
                return reader.Value;
            }
            object returnvalue = null;
            if (objectType == typeof(DateTime)) returnvalue = DateTime.MinValue;
            var str = reader.Value != null ? reader.Value.ToString() : string.Empty;
            DateTime value;
            if (string.IsNullOrEmpty(str))
            {
                return returnvalue;
            }

            if (DateTime.TryParseExact(str, formats, null, DateTimeStyles.None, out value))
            {
                return value;
            }
            return returnvalue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(string.Format("{0:yyyy-MM-dd HH:mm:ss}", value));
        }
    }
}
