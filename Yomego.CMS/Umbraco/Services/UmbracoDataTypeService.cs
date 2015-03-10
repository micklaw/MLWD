using System.Collections.Generic;
using System.Xml.XPath;

namespace Yomego.CMS.Umbraco.Services
{
    public class UmbracoDataTypeService : DataTypeService
    {
        public override Dictionary<string, string> GetPreValue(int id)
        {
            var models = new Dictionary<string, string>();

            XPathNodeIterator nodes = umbraco.library.GetPreValues(id);

            nodes.MoveNext();

            XPathNodeIterator iterator = nodes.Current.SelectChildren("preValue", "");

            while (iterator.MoveNext())
            {
                models.Add(iterator.Current.Value, iterator.Current.Value);
            }

            return models;
        }
    }
}