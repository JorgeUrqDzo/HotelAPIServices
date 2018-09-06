using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Hotel.WebApi.Json
{
    public class DateOnlyConverter : JsonConverter
    {
        private readonly string[] SupportedFormats = {"M-d-yy", "M-d-yyyy", "M/d/yy", "M/d/yyyy", "yyyy-MM-dd"};

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if ((reader.Value == null || reader.Value.ToString() == "") && objectType == typeof(DateTime?)) return null;

            var date = reader.Value is DateTime
                ? (DateTime) reader.Value
                : DateTime.ParseExact(reader.Value as string, SupportedFormats, null,
                    DateTimeStyles.None);

            return date.Date;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var d = (DateTime) value;
            writer.WriteValue(d.ToString("yyyy-MM-ddT00:00:00"));
        }
    }
}