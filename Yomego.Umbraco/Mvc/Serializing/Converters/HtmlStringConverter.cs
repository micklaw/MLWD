using System;
using System.Web;
using Newtonsoft.Json;

namespace Yomego.Umbraco.Mvc.Serializing.Converters
{
    public class HtmlStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IHtmlString).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = value as IHtmlString;

            if (source == null)
            {
                return;
            }

            writer.WriteValue(source.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            HtmlString htmlString = null;

            if (reader != null)
            {
                var value = reader.Value as string;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    htmlString = new HtmlString(value);
                }
            }

            return htmlString;
        }
    }
}
