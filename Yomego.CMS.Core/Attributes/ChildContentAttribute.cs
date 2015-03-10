using System;

namespace Yomego.CMS.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ChildContentAttribute : Attribute
    {
        private Type _searchType { get; set; }

        public Type SearchType
        {
            get { return _searchType; }
        }

        public ChildContentAttribute()
        {
            
        }

        public ChildContentAttribute(Type searchType)
        {
            _searchType = searchType;
        }
    }
}
