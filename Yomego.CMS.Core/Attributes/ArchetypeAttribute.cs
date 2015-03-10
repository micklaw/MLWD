using System;

namespace Yomego.CMS.Core.Attributes
{
    public class ArchetypeAttribute : Attribute
    {
        public Type ListType { get; set; }

        public ArchetypeAttribute()
        {
            
        }

        public ArchetypeAttribute(Type listType) : this()
        {
            ListType = listType;
        }
    }
}
