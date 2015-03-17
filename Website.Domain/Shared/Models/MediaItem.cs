using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLWD.Umbraco.Umbraco.ModelBuilder.Attributes;

namespace Website.Domain.Shared.Models
{
    public class MediaItem
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int Bytes { get; set; }

        public string Name { get; set; }

        public virtual bool HasUrl
        {
            get { return !string.IsNullOrWhiteSpace(Url); }
        }
    }
}
