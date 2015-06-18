using System.ComponentModel;
using Archetype.Models;
using Website.Domain.Shared.Ditto.TypeConverters;
using Yomego.Umbraco.Mvc.Model.Media;

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