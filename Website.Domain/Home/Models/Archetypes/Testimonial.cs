using System.ComponentModel;
using Archetype.Models;
using Website.Domain.Shared.Converters;
using Website.Domain.Shared.Models;

namespace Website.Domain.Home.Models.Archetypes
{
    public class Testimonial : ArchetypeFieldsetModel
    {
        public string Person { get; set; }

        [TypeConverter(typeof(ImageConverter))]
        public Image Image { get; set; }

        public string Quote { get; set; }
    }
}