namespace Yomego.Umbraco.Mvc.Controllers.CMS.Api
{
    public class BaseApiController : UmbracoApiController<Yomego.Umbraco.App>
    {
        public int FixPage(int? page)
        {
            if (!page.HasValue)
            {
                return 0;
            }

            var finalPage = page.Value - 1;

            return (finalPage < 0) ? 0 : finalPage;
        }
    }
}
