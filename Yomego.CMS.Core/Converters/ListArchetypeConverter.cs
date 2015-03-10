using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vega.USiteBuilder.DocumentTypeBuilder;
using Yomego.CMS.Core.Umbraco.Model;

namespace Yomego.CMS.Core.Converters
{
    public class ListArchetypeConverter : Newtonsoft.Json.Converters.CustomCreationConverter<List<ArchetypeContent>>
    {
        public List<ArchetypeContent> Create(JArray jObject, JsonSerializer serializer)
        {
            var content = new List<ArchetypeContent>();

            if (jObject != null)
            {
                foreach (var item in jObject)
                {
                    var type = item.Value<string>("ArchetypeAlias");

                    if (!string.IsNullOrWhiteSpace(type))
                    {
                        Type entityType = DocumentTypeManager.GetArchetype(type);

                        if (entityType != null)
                        {
                            var entity = Activator.CreateInstance(entityType) as ArchetypeContent;

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
            List<ArchetypeContent> target = null;

            if (reader != null)
            {
                try
                {
                    var jObject = JArray.Load(reader);

                    target = Create(jObject, serializer);
                }
                catch (JsonReaderException)
                {
                    //[ML] - Yeah whatever, were suppressing this, but it stops the site falling down on deployments
                }
            }

            return target;
        }

        public override List<ArchetypeContent> Create(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
