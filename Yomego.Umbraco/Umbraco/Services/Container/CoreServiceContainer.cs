using Yomego.Umbraco.Umbraco.Services.Content;
using Yomego.Umbraco.Umbraco.Services.DataTypes;

namespace Yomego.Umbraco.Umbraco.Services.Container
{
    public class CoreServiceContainer : Yomego.Umbraco.Context.Container
    {
        public ContentService Content { get { return Get<ContentService>(); } }

        public DataTypeService DataType { get { return Get<DataTypeService>(); } }
    }
}
