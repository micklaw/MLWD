using System;

namespace Yomego.CMS.Core.Attributes
{
    public class ArchetypeContentAttribute : Attribute
    {
        public string Alias { get; set; }

        public ArchetypeContentAttribute()
        {
            
        }

        public ArchetypeContentAttribute(string alias)
            : this()
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                throw new NullReferenceException("Alias cannot be null or empty");
            }

            Alias = alias;
        }
    }
}