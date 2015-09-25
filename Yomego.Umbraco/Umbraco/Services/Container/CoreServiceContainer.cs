using Yomego.Umbraco.Umbraco.Services.Content;
using Yomego.Umbraco.Umbraco.Services.DataTypes;

namespace Yomego.Umbraco.Umbraco.Services.Container
{
    public class CoreServiceContainer : Context.Container
    {
        public ContentService Content => Get<ContentService>();

        public DataTypeService DataType => Get<DataTypeService>();
    }
}
