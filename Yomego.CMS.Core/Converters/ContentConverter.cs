using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vega.USiteBuilder.DocumentTypeBuilder;
using Yomego.CMS.Core.Umbraco.Model;

namespace Yomego.CMS.Core.Converters
{
    public class ContentConverter : Newtonsoft.Json.Converters.CustomCreationConverter<Content>
    {
        public Content Create(JObject jObject)
        {
            var type = (string)jObject.Property("ContentTypeAlias");

            if (!string.IsNullOrWhiteSpace(type))
            {
                Type entityType = DocumentTypeManager.GetDocumentTypeType(type);

                if (entityType == null)
                {
                    throw new ApplicationException(string.Format("The type {0} is not a valid document type.", type));
                }

                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.None
                };

                settings.Converters.Add(new HtmlStringConverter());
                settings.Converters.Add(new ListContentConverter());
                settings.Converters.Add(new ListArchetypeConverter());

                return JsonConvert.DeserializeObject(jObject.ToString(), entityType, settings) as Content;
            }

            return null;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Content target = null;

            if (reader != null)
            {
                var jObject = JObject.Load(reader);

                target = Create(jObject);
            }

            return target;
        }

        public override Content Create(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
