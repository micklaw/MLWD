using System.Web.Http;

namespace MLWD.Umbraco.Mvc.Controllers.CMS.Api
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
