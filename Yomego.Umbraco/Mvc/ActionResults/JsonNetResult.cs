using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Archetype.Models;
using Newtonsoft.Json;
using umbraco.presentation.channels.businesslogic;
using Umbraco.Core.Models.PublishedContent;
using Yomego.Umbraco.Mvc.Serializing;
using Yomego.Umbraco.Mvc.Serializing.Converters;

namespace Yomego.Umbraco.Mvc.ActionResults
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult(object data, string contentType = "application/json", Encoding contentEncoding = null, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            base.Data = data ?? new {};
            base.ContentType = !string.IsNullOrEmpty(contentType) ? contentType : "application/json";
            base.ContentEncoding = contentEncoding ?? Encoding.UTF8;
            base.JsonRequestBehavior = behavior;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var response = context.HttpContext.Response;

            response.ContentType = ContentType;
            response.ContentEncoding = ContentEncoding;

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new ExcludeContractResolver(new Dictionary<Type, IList<string>>
            {
                {
                    typeof (PublishedContentModel), new List<string>()
                    {
                        "Children",
                        "ContentSet",
                        "ContentType",
                        "ItemType",
                        "Parent",
                        "Properties",
                        "properties",
                        "this",
                        "Content"
                    }
                },
                {
                    typeof (ArchetypeFieldsetModel), new List<string>()
                    {
                        "Properties",
                        "properties"
                    }
                }
            }),

                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };

            settings.Converters.Add(new HtmlStringConverter());

            // If you need special handling, you can call another form of SerializeObject below
            var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented, settings);

            response.Write(serializedObject);
        }
    }
}
