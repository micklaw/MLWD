using System.Web.Http;

namespace Yomego.Umbraco.Mvc.Controllers.CMS.Api
{
    public class DataTypeApiController : BaseApiController
    {
        [HttpGet]
        public object GetPreValues(int id)
        {
            return App.Services.DataType.GetPreValue(id);
        }
    }
}
