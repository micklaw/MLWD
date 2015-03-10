using System;
using System.Collections.Generic;

namespace Yomego.CMS.Core.Umbraco.Model
{
    /// <summary>
    /// Base class for all document types.
    /// </summary>
    public class Content
    {
        public int Id { get; set; }

        public int SortOrder { get; set; }

        public string Name { get; set; }

        public string ContentTypeAlias { get; set; }

        public int ParentId { get; set; }

        public string ParentUrl { get; set; }

        public string ParentName { get; set; }

        public string Path { get; set; }

        public virtual string Url { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
 
        public IList<Content> Children { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
