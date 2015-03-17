using System;

namespace MLWD.Umbraco.Umbraco.ModelBuilder.Attributes
{
    /// <summary>
    /// The Ditto ignore property attribute. Used for specifying that Umbraco should
    /// ignore this property during conversion.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DittoIgnoreAttribute : Attribute
    {
    }
}