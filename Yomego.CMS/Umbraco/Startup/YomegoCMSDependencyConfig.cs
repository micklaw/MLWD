using Yomego.CMS.Context;
using Yomego.CMS.Umbraco.Services;

namespace Yomego.CMS.Umbraco.Startup
{
    public class YomegoCMSDependencyConfig
    {
        public static void Register()
        {
            // Hook up Umbraco plugin
            App.ResolveUsing<ContentService, UmbracoContentService>();
            App.ResolveUsing<MediaService, UmbracoMediaService>();
            App.ResolveUsing<DataTypeService, UmbracoDataTypeService>();
        }
    }
}