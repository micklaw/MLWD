using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Website.Domain.Shared.Serializing.Converters
{
    public class HtmlStringConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IHtmlString).IsAssignableFrom(objectType);
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var source = value as IHtmlString;

            if (source == null)
            {
                return;
            }

            writer.WriteValue(source.ToString());
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
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
