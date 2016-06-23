using System;
using System.Globalization;

namespace TweetCollector.Utility
{
    public class TwitterDateTimeConverter : Newtonsoft.Json.Converters.DateTimeConverterBase
    {
        /// <summary>
        /// The date pattern for most dates returned by the API
        /// </summary>
        protected const string DateFormat = "ddd MMM dd HH:mm:ss zz00 yyyy";

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.Value == null)
                return new ArgumentException(nameof(reader) + ".Value is null", nameof(reader));

            if (reader.Value.GetType() != typeof (string))
                return new ArgumentException(nameof(reader) + ".Value is not a string", nameof(reader));

            var dateTimeOffset = DateTimeOffset.ParseExact((string) reader.Value, DateFormat, new CultureInfo("en-US"));
            return dateTimeOffset;
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value.GetType() != typeof (DateTimeOffset))
                throw new ArgumentException(nameof(value) + " is not a DateTimeOffset", nameof(value));

            writer.WriteValue(((DateTimeOffset) value).ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }
}
