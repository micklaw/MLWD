using umbraco.MacroEngines;

namespace Yomego.Umbraco.Utils
{
    public static class DynamicMediaUtils
    {
        public static string GetPropertyAsString(this DynamicMedia node, string propertyName)
        {
            var property = node.GetProperty(propertyName);
            if (property != null)
                return property.Value;
            else
                return null;
        }
    }
}
