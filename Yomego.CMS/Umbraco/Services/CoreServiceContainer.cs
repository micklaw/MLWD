using Yomego.CMS.Context;
using Yomego.CMS.Core.Context;

namespace Yomego.CMS.Umbraco.Services
{
    public class CoreServiceContainer : Container
    {
        public ContentService Content { get { return Get<ContentService>(); } }

        public MediaService Media { get { return Get<MediaService>(); } }

        public DataTypeService DataType { get { return Get<DataTypeService>(); } }
    }
}
