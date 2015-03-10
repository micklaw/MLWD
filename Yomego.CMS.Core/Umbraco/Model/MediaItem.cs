using System.Collections.Generic;

namespace Yomego.CMS.Core.Umbraco.Model
{
    public class MediaItem
    {
        public MediaItem()
        {
            Children = new List<MediaItem>();    
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public string Path { get; set; }

        public int Level { get; set; }

        public IList<MediaItem> Children { get; set; }
    }
}
