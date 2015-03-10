using Yomego.CMS.Core.Attributes;
using System;
using Yomego.CMS.Core.Enums;

namespace Vega.USiteBuilder.MemberBuilder
{
    /// <summary>
    /// Declares a member type property. Use this property in MemberType definition class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MemberTypePropertyAttribute : ContentTypePropertyAttribute
    {
        /// <summary>
        /// Declares a member type property. Use this property in MemberType definition class
        /// </summary>
        /// <param name="type">Property data type</param>
        public MemberTypePropertyAttribute(ContentTypePropertyUI type)
            : base (type)
        {
        }
    }
}
