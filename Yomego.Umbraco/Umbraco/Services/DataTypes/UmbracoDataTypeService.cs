using System.Collections.Generic;
using System.Xml.XPath;
using umbraco;

namespace Yomego.Umbraco.Umbraco.Services.DataTypes
{
    internal class UmbracoDataTypeService : DataTypeService
    {
        public override Dictionary<string, string> GetPreValue(int id)
        {
            var models = new Dictionary<string, string>();

            XPathNodeIterator nodes = library.GetPreValues(id);

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