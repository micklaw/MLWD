using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vega.USiteBuilder.DocumentTypeBuilder;
using Yomego.CMS.Core.Umbraco.Model;

namespace Yomego.CMS.Core.Converters
{
    public class ListContentConverter : Newtonsoft.Json.Converters.CustomCreationConverter<List<Content>>
    {
        public List<Content> Create(JArray jObject, JsonSerializer serializer)
        {
            var content = new List<Content>();

            if (jObject != null)
            {
                foreach (var item in jObject)
                {
                    var type = item.Value<string>("ContentTypeAlias");

                    if (!string.IsNullOrWhiteSpace(type))
                    {
                        Type entityType = DocumentTypeManager.GetDocumentTypeType(type);

                        if (entityType != null)
                        {
                            var entity = Activator.CreateInstance(entityType) as Content;

                            serializer.Populate(item.CreateReader(), entity);

                            content.Add(entity);
                        }
                    }
                }
            }

            return content;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<Content> target = null;

            if (reader != null)
            {
                var jObject = JArray.Load(reader);

                target = Create(jObject, serializer);
            }

            return target;
        }

        public override List<Content> Create(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
