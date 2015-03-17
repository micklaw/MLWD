using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MLWD.Umbraco.Mvc.Controllers.ActionResults
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult(object data, string contentType, Encoding contentEncoding)
        {
            base.Data = data;
            base.ContentType = contentType;
            base.ContentEncoding = contentEncoding;
        }

        public JsonNetResult(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
            : this(data, contentType, contentEncoding)
        {
            base.JsonRequestBehavior = behavior;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            // If you need special handling, you can call another form of SerializeObject below
            var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented);

            response.Write(serializedObject);
        }
    }
}
