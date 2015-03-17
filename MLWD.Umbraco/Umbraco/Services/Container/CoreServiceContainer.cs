using MLWD.Umbraco.Umbraco.Services.Content;
using MLWD.Umbraco.Umbraco.Services.DataTypes;

namespace MLWD.Umbraco.Umbraco.Services.Container
{
    public class CoreServiceContainer : Context.Container
    {
        public ContentService Content { get { return Get<ContentService>(); } }

        public DataTypeService DataType { get { return Get<DataTypeService>(); } }
    }
}
