using MLWD.Umbraco.Umbraco.Services.Content;
using MLWD.Umbraco.Umbraco.Services.DataTypes;

namespace MLWD.Umbraco.Umbraco.Startup
{
    public class MLWDDependencyConfig
    {
        public static void Register()
        {
            // Hook up Umbraco plugin
            App.ResolveUsing<ContentService, UmbracoContentService>();
            App.ResolveUsing<DataTypeService, UmbracoDataTypeService>();
        }
    }
}