using System.Web.Http;

namespace Yomego.CMS.Mvc.Controllers
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
